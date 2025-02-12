using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using Microsoft.Extensions.Logging;

using SQLite;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    public class PlaylistRepository(IDbContext _dbContext,
                                    ILogger _logger)
               : IPlaylistRepository
    {

        public List<RecentPlaylist> GetPlaylists(string userId, PlaylistType playlistType, bool isSaved)
        {
      
           var connection = _dbContext.GetConnection().Result;
            if (connection != null)
            {
                var sql = RepositoryHelper.GetPlaylists;
                return connection.QueryAsync<RecentPlaylist>(sql, userId, playlistType, isSaved).Result;
            }
            _logger.LogError("Could not access db connection");

            return [];
        }

        public int AddPlaylists(List<RecentPlaylist> playlists, string userId, bool isSaved)
        {
            var count = 0;

           var connection = _dbContext.GetConnection().Result;
            if (connection != null)
            {
                foreach(var playlist in playlists) 
                {
                    if (playlist.Id != null)
                    {
                        count += AddPlaylist(connection, playlist);
                        RepositoryHelper.UpdateLastAccessed(connection, userId, playlist.Id, DateTime.Today, isSaved);
                    }
                }
            }
            _logger.LogError("Could not access db connection");

            return count;
        }

        private int AddPlaylist(SQLiteAsyncConnection connection, RecentPlaylist playlist)
        {
            var found = connection.Table<Playlist>()
                                   .FirstOrDefaultAsync(p => p.Id == playlist.Id)
                                   .Result;
            if (found == null)
            {
                return connection.InsertAsync(playlist.ToPlaylist()).Result;
            }
            return 0;
        }

        public bool RemoveSaved(string userId, string playlistId)
        {
           var connection = _dbContext.GetConnection().Result;
            if (connection != null)
            {
                // check to see it albums is indeed saved
                var playlist = connection.Table<RecentItem>()
                                         .FirstOrDefaultAsync(ri => ri.ItemId == playlistId &&
                                                                    ri.UserId == userId &&
                                                                    ri.IsSaved)
                                         .Result;
                if (playlist == null)
                {
                    return true;
                }

                // make sure no aartists have dibs on it
                var sql = RepositoryHelper.CheckIfPlaylistBelongsToAnArtist;
                var artistId = connection.QueryScalarsAsync<string>(sql, playlistId).Result;
                if (artistId == null)
                {
                    connection.Table<RecentItem>()
                              .DeleteAsync(ri => ri.UserId == userId &&
                                                 ri.ItemId != playlistId);
                    _logger.LogDebug($"did not delete playlist '{playlist}' because it is owned by artist '{artistId}'");
                    return true;
                }

                // we are ok top continue... remove any unshared tracks
                RepositoryHelper.RemovePlaylistTracks(connection, playlistId);

                // remove playlist and and associated records
                RepositoryHelper.RemovePlaylists(connection, userId, new List<string> { playlistId });

            }
            _logger.LogError("Could not access db connection");
            return false;

        }

        public bool RemoveUnsavedPlaylist(string userId, string artistId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUnsavedPlaylists(string userId, PlaylistType playlistType)
        {
            throw new NotImplementedException();
        }
    }
}
