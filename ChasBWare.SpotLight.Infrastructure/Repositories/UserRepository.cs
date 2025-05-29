using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories;

public class UserRepository(IDbContext _dbContext,
                            ILogger<IArtistRepository> _logger)
           : IUserRepository
{
    public List<Playlist> LoadUserAlbums(string userId)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            var sql = RepositoryHelper.GetUserAlbums;
            var playlists = connection.QueryAsync<Playlist>(sql, userId).Result;
        }
        _logger.LogError("Could not access db connection");
        return [];
    }

    public User? FindUser(string userId)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            return connection.Table<User>()
                             .FirstOrDefaultAsync(a => a.Id == userId)
                             .Result;
        }
        _logger.LogError("Could not access db connection");
        return null;
    }

    public void StoreUserAndAlbums(User user, List<Playlist> albums)
    {
        if (user.Id == null)
        {
            return;
        }

        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            user.LastAccessed = DateTime.Now;

            var existingArtist = connection.Table<User>()
                                           .FirstOrDefaultAsync(a => a.Id == user.Id)
                                           .Result;
            if (existingArtist == null)
            {
                connection.InsertAsync(user);
            }
            else
            {
                connection.UpdateAsync(user);
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

                var existingLink = connection.Table<OwnedPlaylist>()
                                             .FirstOrDefaultAsync(ap => ap.OwnerId == user.Id &&
                                                                        ap.PlaylistId == album.Id)
                                             .Result;
                if (existingLink == null)
                {
                    connection.InsertAsync(new OwnedPlaylist { OwnerId = user.Id, PlaylistId = album.Id });
                }
            }
        }
        _logger.LogError("Could not access db connection");
    }

    public void UpdateLastAccessed(User user)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            user.LastAccessed = DateTime.Now;
            if (connection.Table<User>().FirstOrDefaultAsync(pl => pl.Id == user.Id) != null)
            {
                connection.UpdateAsync(user);
            }
            else
            {
                connection.InsertAsync(user);
            }
        }
        _logger.LogError("Could not access db connection");
    }
}
