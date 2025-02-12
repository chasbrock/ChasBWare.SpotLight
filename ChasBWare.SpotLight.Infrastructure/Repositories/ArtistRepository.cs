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
        public int Add(Artist artist)
        {
            var connection =  _dbContext.GetConnection().Result;
            if (connection != null)
            {
                var found = connection.Table<Artist>()
                                       .FirstOrDefaultAsync(a => a.Id == artist.Id).Result;
                if (found == null)
                {
                    return connection.InsertAsync(artist).Result;
                }
                else
                {
                    return connection.UpdateAsync(artist).Result;
                }
            }
            _logger.LogError("Could not access db connection");

            return -1;
        }


        public Artist? FindArtist(string artistId)
        {
            var connection = _dbContext.GetConnection().Result;
            if (connection != null)
            {
                return connection.Table<Artist>()
                                 .FirstOrDefaultAsync(a => a.Id == artistId).Result;
             
            }
            _logger.LogError("Could not access db connection");
            return null;
        }

        public bool RemoveUnsavedArtist(string userId, string artistId)
        {  
            var connection = _dbContext.GetConnection().Result;
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

        public bool RemoveUnsavedArtists(string userId)
        {
           var connection = _dbContext.GetConnection().Result;
            if (connection != null)
            {
                try
                {
                    foreach (var sql in RepositoryHelper.DeleteAllRecentArtists)
                    {
                        connection.ExecuteAsync(sql);
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

        public List<Tuple<Artist, DateTime>> GetRecentArtists(string userId)
        {
           var connection = _dbContext.GetConnection().Result;
            if (connection != null)
            {
                var sql = RepositoryHelper.GetRecentArtists;
                var found = connection.QueryAsync<(string Id, string Name, string Image, DateTime LastAccessed)>(sql, userId).Result;
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

        public List<RecentPlaylist> LoadArtistAlbums(string artistId)
        {
           var connection = _dbContext.GetConnection().Result;
            if (connection != null)
            {
                var sql = RepositoryHelper.GetArtistAlbums;
                return connection.QueryAsync<RecentPlaylist>(sql, artistId).Result;
            }
            _logger.LogError("Could not access db connection");
            return [];
        }

        public int LinkAlbumToArtist(string artistId, string playListId)
        {
           var connection = _dbContext.GetConnection().Result;
            if (connection != null)
            {
                var found = connection.Table<ArtistPlaylist>()
                                      .FirstOrDefaultAsync(ap => ap.ArtistId == artistId &&
                                                                 ap.PlaylistId == playListId).Result;
                if (found == null)
                {
                    return connection.InsertAsync(new ArtistPlaylist { ArtistId = artistId, PlaylistId = playListId }).Result;
                }
            }
            _logger.LogError("Could not access db connection");
            return -1;
        }

        public void AddRecentArtistAndAlbums(string currentUserId, Artist artist, List<RecentPlaylist> albums)
        {
            if (artist.Id == null)
            {
                return;
            }

           var connection = _dbContext.GetConnection().Result;
            if (connection != null)
            {
                var existingArtist = connection.Table<Artist>()
                                               .FirstOrDefaultAsync(a => a.Id == artist.Id)
                                               .Result;
                if (existingArtist == null)
                {
                    connection.InsertAsync(artist);
                }

               RepositoryHelper.UpdateLastAccessed(connection, currentUserId, artist.Id, DateTime.Now, true);

                foreach (var album in albums) 
                {
                    var existingPlaylist = connection.Table<Playlist>()
                                                      .FirstOrDefaultAsync(p => p.Id == album.Id)
                                                      .Result;
                    if (existingPlaylist == null)
                    {
                        connection.InsertAsync(album.ToPlaylist());
                    }

                    var existingLink = connection.Table<ArtistPlaylist>()
                                                 .FirstOrDefaultAsync(ap => ap.ArtistId == artist.Id &&
                                                                            ap.PlaylistId == album.Id)
                                                 .Result;
                    if (existingLink == null)
                    {
                        connection.InsertAsync(new ArtistPlaylist { ArtistId = artist.Id, PlaylistId = album.Id });
                    }
                }
            }
            _logger.LogError("Could not access db connection");

        }
    
    }
}