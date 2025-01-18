using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    public class PlaylistRepository(IDbContext _dbContext,
                                    Logger<PlaylistRepository> _logger)
               : IPlaylistRepository
    {

        public async Task<List<Tuple<Playlist, DateTime>>> GetPlaylists(string? userId, PlaylistType playlistType, bool isSaved)
        {
            if (userId == null) 
            {
                return [];
            }

            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var sql = RepositoryHelper.GetPlaylists;
                var found = await connection.QueryAsync<(string Id, string Name, string Owner, string Description,
                                                            string Image, PlaylistType PlaylistType, DateTime ReleaseDate, string Uri,
                                                            DateTime LastAccessed)>(sql, userId, playlistType, isSaved);
                if (found != null)
                {
                    return found.Select(r => new Tuple<Playlist, DateTime>(new Playlist
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Owner = r.Owner,
                        Description = r.Description,
                        Image = r.Image,
                        PlaylistType = r.PlaylistType,
                        ReleaseDate = r.ReleaseDate,
                        Uri = r.Uri
                    },
                    r.LastAccessed)).ToList();
                }
            }
            _logger.LogError("Could not access db connection");

            return [];
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
