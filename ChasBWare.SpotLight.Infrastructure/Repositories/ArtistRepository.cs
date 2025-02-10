using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
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
                if (found == null)
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


        public async Task<Artist?> FindArtist(string artistId)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                return await connection.Table<Artist>()
                                       .FirstOrDefaultAsync(a => a.Id == artistId);
             
            }
            _logger.LogError("Could not access db connection");
            return null;
        }

        public async Task<bool> Remove(string userId, string artistId)
        {  
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                //remove playlists for artis that are not shared
          /*      var sql = RepositoryHelper.GetDeleteableArtistPlaylists;
                var deleteablePlaylists = await connection.QueryScalarsAsync<string>(sql, artistId, userId);
                foreach (var playlistId in deleteablePlaylists)
                {
                   await  RepositoryHelper.RemovePlaylistTracks(connection, playlistId);
                }

                await RepositoryHelper.RemovePlaylists(connection, userId, deleteablePlaylists);
                await RepositoryHelper.RemoveArtist(connection, userId, artistId);           
                 */
                return true;
            }
            _logger.LogError("Could not access db connection");

            return false;
        }

        public async Task<bool> RemoveAll(string userId)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                try
                {
                    foreach (var sql in RepositoryHelper.DeleteAllRecentArtists)
                    {
                        await connection.ExecuteAsync(sql);
                    }
                    return true;
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, "Remove all recent artists");
                }
            }
            _logger.LogError("Could not access db connection");

            return false;
        }



        public async Task<List<Tuple<Artist, DateTime>>> GetRecentArtists(string userId)
        {
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

        public async Task AddRecentArtistAndAlbums(string currentUserId, Artist artist, List<RecentPlaylist> albums)
        {
            if (artist.Id == null)
            {
                return;
            }

            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var existingArtist = await connection.Table<Artist>()
                                                     .FirstOrDefaultAsync(a => a.Id == artist.Id);
                if (existingArtist == null)
                {
                    await connection.InsertAsync(artist);
                }

               await RepositoryHelper.UpdateLastAccessed(connection, currentUserId, artist.Id, DateTime.Now, true);

                foreach (var album in albums) 
                {
                    var existingPlaylist = await connection.Table<Playlist>()
                                                           .FirstOrDefaultAsync(p => p.Id == album.Id);
                    if (existingPlaylist == null)
                    {
                        await connection.InsertAsync(album.ToPlaylist());
                    }

                    var existingLink = await connection.Table<ArtistPlaylist>()
                                                       .FirstOrDefaultAsync(ap => ap.ArtistId == artist.Id &&
                                                                                  ap.PlaylistId == album.Id);
                    if (existingLink == null)
                    {
                        await connection.InsertAsync(new ArtistPlaylist { ArtistId = artist.Id, PlaylistId = album.Id });
                    }
                }
            }
            _logger.LogError("Could not access db connection");

        }
    
    }
}