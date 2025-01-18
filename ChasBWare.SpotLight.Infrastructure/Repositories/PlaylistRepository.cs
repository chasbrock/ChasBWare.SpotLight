using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{

    public class Pair<TX, TY> 
    {
        public TX? X { get; set; }
        public TY? Y { get; set; }
    }


    public class PlaylistRepository(IDbContext _dbContext,
                                    ILogger _logger)
               : IPlaylistRepository
    {

        public async Task<List<Tuple<Playlist, DateTime>>> GetPlaylists(string? userId, PlaylistType playlistType, bool isSaved)
        {
            if (userId == null) 
            {
                return [];
            }

            List<Tuple<Playlist, DateTime>> items = [];
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var sql = RepositoryHelper.GetPlaylists;
                var found = await connection.QueryAsync<Pair<Playlist, DateTime>>(sql, userId, playlistType, isSaved);
                if (found != null)
                {
                    foreach (var pair in found)
                    {
                        if (pair.X != null)
                        {
                            items.Add(new Tuple<Playlist, DateTime>(pair.X, pair.Y));
                        }
                    }
                }
            }
            _logger.LogError("Could not access db connection");

            return items;
        }

        public async Task<int> UpdateLastAccessed(string userId, string playlistId, DateTime lastAccessed)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var recentItem = await connection.Table<RecentItem>()
                                                 .FirstOrDefaultAsync(ri => ri.UserId == userId &&
                                                                            ri.ItemId == playlistId);
                if (recentItem == null)
                {
                    // not IsSaved is always false for artists, simply not the 
                    // way spotify works
                    return await connection.InsertAsync(new RecentItem { UserId = userId, ItemId = playlistId, LastAccessed = lastAccessed });
                }
                recentItem.LastAccessed = lastAccessed;
                return await connection.UpdateAsync(recentItem);
            }
            _logger.LogError("Could not access db connection");
            return -1;
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
