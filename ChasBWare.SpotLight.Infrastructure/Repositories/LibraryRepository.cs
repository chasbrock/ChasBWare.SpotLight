using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Models;
using Microsoft.Extensions.Logging;
using SQLite;

namespace ChasBWare.SpotLight.Infrastructure.Repositories;

public class LibraryRepository(IDbContext _dbContext,
                               ILogger<LibraryRepository> _logger)
           : ILibraryRepository
{
    public int AddPlaylists(List<Playlist> playlists)
    {
         var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            var count = 0;
            foreach (var playlist in playlists)
            {
                if (playlist.Id != null)
                {
                    var found = connection.Table<Playlist>()
                                  .FirstOrDefaultAsync(p => p.Id == playlist.Id)
                                  .Result;
                    if (found == null)
                    {
                        count += connection.InsertAsync(playlist).Result;
                    }

                    var link = connection.Table<LibraryItem>()
                                 .FirstOrDefaultAsync(p => p.Id == playlist.Id)
                                 .Result;
                    if (link == null)
                    {
                        count += connection.InsertAsync(new LibraryItem { Id = playlist.Id}).Result;
                    }
                }
            }
            return count;

        }
        _logger.LogError("Could not access db connection");

        return -1;
    }

    public Playlist? FindPlaylist(string playlistId)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            return connection.Table<Playlist>()
                             .FirstOrDefaultAsync(pl => pl.Id == playlistId)
                             .Result;
        }
        return null;
    }

    public HashSet<string> GetPlaylistIds()
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            var sql = RepositoryHelper.GetLibraryItemIds;
            return connection.QueryScalarsAsync<string>(sql).Result.ToHashSet();
        }
        _logger.LogError("Could not access db connection");

        return [];
    }

    public List<Playlist> GetPlaylists(PlaylistType playlistType)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            var sql = RepositoryHelper.GetLibraryItems;
            return connection.QueryAsync<Playlist>(sql, playlistType).Result;
        }
        _logger.LogError("Could not access db connection");

        return [];
    }

    public bool RemovePlaylist(string playlistId)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            var sql = RepositoryHelper.DeleteLibraryItem;
            return connection.ExecuteAsync(sql, playlistId).Result != 0;
        }
        _logger.LogError("Could not access db connection");
        return false;
    }

    public bool TransferPlaylistToLibrary(Playlist playlist, bool save)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            if (save)
            {
                playlist.LastAccessed = DateTime.Now;
                if (connection.Table<Playlist>().FirstOrDefaultAsync(pl => pl.Id == playlist.Id).Result == null)
                {
                    connection.InsertAsync(playlist);
                }
                else
                {
                    connection.UpdateAsync(playlist);
                }

                if (connection.Table<LibraryItem>().FirstOrDefaultAsync(pl => pl.Id == playlist.Id).Result == null)
                {
                    connection.InsertAsync(new LibraryItem { Id = playlist.Id });
                }
                return true;
            }
            else 
            {
                connection.DeleteAsync(new LibraryItem { Id = playlist.Id });
                return true;
            }
        }
        _logger.LogError("Could not access db connection");
        return false;
    }

    public void UpdateLastAccessed(Playlist playlist)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            playlist.LastAccessed = DateTime.Now;
            if (connection.Table<Playlist>().FirstOrDefaultAsync(pl => pl.Id == playlist.Id) != null)
            {
                connection.UpdateAsync(playlist);
            }
            else
            {
                connection.InsertAsync(playlist);
            }
            return;
        }
        _logger.LogError("Could not access db connection");
       
    }

}
