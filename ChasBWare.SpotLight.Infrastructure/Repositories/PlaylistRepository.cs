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

        private static Random randy = new Random();

        public async Task<List<RecentPlaylist>> GetPlaylists(string userId, PlaylistType playlistType, bool isSaved)
        {
      
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var sql = RepositoryHelper.GetPlaylists;
                return await connection.QueryAsync<RecentPlaylist>(sql, userId, playlistType, isSaved);
            }
            _logger.LogError("Could not access db connection");

            return [];
        }

        public async Task<int> AddPlaylists(List<RecentPlaylist> playlists, string userId, bool isSaved)
        {
            var count = 0;

            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                foreach(var playlist in playlists) 
                {
                    if (playlist.Id != null)
                    {
                        count += await AddPlaylist(connection, playlist);

                        // TODO remove this bit
                        var lastAccessed = DateTime.Today.AddDays(-randy.NextDouble() * 40);

                        await RepositoryHelper.UpdateLastAccessed(connection, userId, playlist.Id, lastAccessed, isSaved);
                    }
                }
            }
            _logger.LogError("Could not access db connection");

            return count;
        }

        private async Task<int> AddPlaylist(SQLiteAsyncConnection connection, RecentPlaylist playlist)
        {
            var found = await connection.Table<Playlist>().FirstOrDefaultAsync(p => p.Id == playlist.Id);
            if (found == null)
            {
                return await connection.InsertAsync(playlist.ToPlaylist());
            }
            return 0;
        }

        public async Task<bool> RemoveSavedItem(string userId, string playlistId)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                // check to see it albums is indeed saved
                var playlist = await connection.Table<RecentItem>()
                                               .FirstOrDefaultAsync(ri => ri.ItemId == playlistId &&
                                                                          ri.UserId == userId &&
                                                                          ri.IsSaved);
                if (playlist == null)
                {
                    return true;
                }

                // make sure no aartists have dibs on it
                var sql = RepositoryHelper.CheckIfPlaylistBelongsToAnArtist;
                var artistId = await connection.QueryScalarsAsync<string>(sql, playlistId);
                if (artistId == null)
                {
                    await connection.Table<RecentItem>()
                                    .DeleteAsync(ri => ri.UserId == userId &&
                                                       ri.ItemId != playlistId);
                    _logger.LogDebug($"did not delete playlist '{playlist}' because it is owned by artist '{artistId}'");
                    return true;
                }

                // we are ok top continue... remove any unshared tracks
                await RepositoryHelper.RemovePlaylistTracks(connection, playlistId);

                // remove playlist and and associated records
                await RepositoryHelper.RemovePlaylists(connection, userId, new List<string> { playlistId });

            }
            _logger.LogError("Could not access db connection");
            return false;

        }
    }
}
