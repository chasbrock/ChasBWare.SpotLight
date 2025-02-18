using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories;

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
                                  .FirstOrDefaultAsync(a => a.Id == artist.Id)
                                  .Result;
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

    public void StoreArtistAndAlbums(Artist artist, List<Playlist> albums)
    {
        if (artist.Id == null)
        {
            return;
        }

       var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            artist.LastAccessed = DateTime.Now;
        
            var existingArtist = connection.Table<Artist>()
                                           .FirstOrDefaultAsync(a => a.Id == artist.Id)
                                           .Result;
            if (existingArtist == null)
            {
                connection.InsertAsync(artist);
            }
            else 
            {
                connection.UpdateAsync(artist);
            }

            foreach (var album in albums)
                {
                    var existingPlaylist = connection.Table<Playlist>()
                                                      .FirstOrDefaultAsync(p => p.Id == album.Id)
                                                      .Result;
                    if (existingPlaylist == null)
                    {
                        connection.InsertAsync(album);
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

    public Artist? FindArtist(string artistId)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            return connection.Table<Artist>()
                             .FirstOrDefaultAsync(a => a.Id == artistId)
                             .Result;
        }
        _logger.LogError("Could not access db connection");
        return null;
    }

    public List<Playlist> LoadArtistAlbums(string artistId)
    {
       var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            var sql = RepositoryHelper.GetArtistAlbums;
            return connection.QueryAsync<Playlist>(sql, artistId).Result;
        }
        _logger.LogError("Could not access db connection");
        return [];
    }

    public void UpdateLastAccessed(Artist artist)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            artist.LastAccessed = DateTime.Now;
            if (connection.Table<Artist>().FirstOrDefaultAsync(pl => pl.Id == artist.Id) != null)
            {
                connection.UpdateAsync(artist);
            }
            else
            {
                connection.InsertAsync(artist);
            }
        }
        _logger.LogError("Could not access db connection");
    }
}