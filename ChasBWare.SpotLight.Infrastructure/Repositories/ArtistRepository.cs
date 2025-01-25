using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{

    public class ArtistRepository(IDbContext _dbContext,
                                  ILogger _logger)
               : IArtistRepository
    {
        public async Task<int> Add(Artist artist)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var found = await connection.Table<Artist>()
                                            .FirstOrDefaultAsync(a => a.Id == artist.Id);
                if (found != null)
                {
                    return await connection.InsertAsync(artist);
                }
                else
                {
                    return await connection.UpdateAsync(artist);
                }
            }
            _logger.LogError("Could not access db connection");

            return -1;
        }

        public async Task<bool> Remove(string? userId, string artistId)
        {
            if (userId == null)
            {
                return false; 
            }
           
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                //remove playlists for artis that are not shared
                var sql = RepositoryHelper.GetDeleteableArtistPlaylists;
                var deleteablePlaylists = await connection.QueryScalarsAsync<string>(sql, artistId, userId);
                foreach (var playlistId in deleteablePlaylists)
                {
                   await  RepositoryHelper.RemovePlaylistTracks(connection, playlistId);
                }

                await RepositoryHelper.RemovePlaylists(connection, userId, deleteablePlaylists);
                await RepositoryHelper.RemoveArtist(connection, userId, artistId);           
                 
                return true;
            }
            _logger.LogError("Could not access db connection");

            return false;
        }

        public async Task<List<Tuple<Artist, DateTime>>> GetRecentArtists(string? userId)
        {
            if (userId == null)
            { 
                return [];
            }

            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var sql = RepositoryHelper.GetRecentArtists;
                var found = await connection.QueryAsync<(string Id, string Name, string Image, DateTime LastAccessed)>(sql, userId);
                if (found != null)
                {
                    return found.Select(r => new Tuple<Artist, DateTime>(
                        new Artist
                        {
                            Id = r.Id,
                            Image = r.Image,
                            Name = r.Name
                        },
                        r.LastAccessed)).ToList();
                }
            }
            _logger.LogError("Could not access db connection");

            return [];
        }

        public async Task<List<RecentPlaylist>> LoadArtistAlbums(string artistId)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var sql = RepositoryHelper.GetArtistAlbums;
                return await connection.QueryAsync<RecentPlaylist>(sql, artistId);
            }
            _logger.LogError("Could not access db connection");
            return [];
        }

        public async Task<int> UpdateLastAccessed(string userId, string artistId, DateTime lastAccessed)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var recentItem = await connection.Table<RecentItem>()
                                                 .FirstOrDefaultAsync(ri => ri.UserId == userId &&
                                                                            ri.ItemId == artistId);
                if (recentItem == null)
                {
                    // not IsSaved is always false for artists, simply not the 
                    // way spotify works
                    return await connection.InsertAsync(
                        new RecentItem
                        {
                            UserId = userId,
                            ItemId = artistId,
                            LastAccessed = lastAccessed
                        });
                }
                recentItem.LastAccessed = lastAccessed;
                return await connection.UpdateAsync(recentItem);
            }
            _logger.LogError("Could not access db connection");
            return -1;
        }

        public async Task<int> LinkAlbumToArtist(string artistId, string playListId)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var found = await connection.Table<ArtistPlaylist>()
                                            .FirstOrDefaultAsync(ap => ap.ArtistId == artistId &&
                                                                       ap.PlaylistId == playListId);
                if (found == null)
                {
                    return await connection.InsertAsync(new ArtistPlaylist { ArtistId = artistId, PlaylistId = playListId });
                }
            }
            _logger.LogError("Could not access db connection");
            return -1;
        }
      
    }
}