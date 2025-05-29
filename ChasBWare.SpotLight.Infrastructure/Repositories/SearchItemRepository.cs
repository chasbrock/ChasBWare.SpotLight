using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories;

public class SearchItemRepository(IDbContext _dbContext,
                                  ILogger<SearchItemRepository> _logger)
           : ISearchItemRepository
{

    public bool AddArtist(Artist artist)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            artist.LastAccessed = DateTime.Now;
            if (connection.Table<Artist>().FirstOrDefaultAsync(a => a.Id == artist.Id).Result == null)
            {
                connection.InsertAsync(artist);
            }
            else
            {
                connection.UpdateAsync(artist);
            }
            if (connection.Table<SearchItem>().FirstOrDefaultAsync(si => si.ItemId == artist.Id).Result == null)
            {
                connection.InsertAsync(new SearchItem { ItemId = artist.Id });
            }
            return true;
        }
        _logger.LogError("Could not access db connection");

        return false;
    }

    public bool AddPlaylist(Playlist playlist)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            playlist.LastAccessed = DateTime.Now;
            if (connection.Table<Playlist>().FirstOrDefaultAsync(a => a.Id == playlist.Id).Result == null)
            {
                connection.InsertAsync(playlist);
            }
            else
            {
                connection.UpdateAsync(playlist);
            }
            if (connection.Table<SearchItem>().FirstOrDefaultAsync(si => si.ItemId == playlist.Id).Result == null)
            {
                connection.InsertAsync(new SearchItem { ItemId = playlist.Id });
            }
            return true;
        }
        _logger.LogError("Could not access db connection");

        return false;
    }

    public bool AddUser(User user)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            user.LastAccessed = DateTime.Now;
            if (connection.Table<User>().FirstOrDefaultAsync(a => a.Id == user.Id).Result == null)
            {
                connection.InsertAsync(user);
            }
            else
            {
                connection.UpdateAsync(user);
            }
            if (connection.Table<SearchItem>().FirstOrDefaultAsync(si => si.ItemId == user.Id).Result == null)
            {
                connection.InsertAsync(new SearchItem { ItemId = user.Id });
            }
            return true;
        }
        _logger.LogError("Could not access db connection");

        return false;
    }


    public List<Artist> GetArtists()
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            var sql = RepositoryHelper.GetSearchArtists;
            return connection.QueryAsync<Artist>(sql).Result;
        }
        _logger.LogError("Could not access db connection");

        return [];
    }

    public List<Playlist> GetPlaylists(PlaylistType playlistType)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            var sql = RepositoryHelper.GetSearchPlaylists;
            return connection.QueryAsync<Playlist>(sql, playlistType).Result;
        }
        _logger.LogError("Could not access db connection");

        return [];
    }

    public List<User> GetUsers()
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            var sql = RepositoryHelper.GetSearchUsers;
            return connection.QueryAsync<User>(sql).Result;
        }
        _logger.LogError("Could not access db connection");

        return [];
    }

    public bool RemoveArtists()
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
                return false;
            }
        }
        _logger.LogError("Could not access db connection");

        return false;
    }


    public bool RemoveArtist(string id)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            try
            {
                foreach (var sql in RepositoryHelper.DeleteRecentArtist)
                {
                    connection.ExecuteAsync(sql);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Remove all recent artists");
                return false;
            }
        }
        _logger.LogError("Could not access db connection");

        return false;
    }

    public bool RemovePlaylist(string playlistId)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            foreach (var sql in RepositoryHelper.DeleteRecentPlaylist)
            {
                connection.ExecuteAsync(sql, playlistId);
            }
            return true;
        }
        _logger.LogError("Could not access db connection");
        return false;
    }

    public bool RemovePlaylists(PlaylistType playlistType)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            foreach (var sql in RepositoryHelper.DeleteAllRecentPlaylists)
            {
                connection.ExecuteAsync(sql, playlistType);
            }
            return true;
        }
        _logger.LogError("Could not access db connection");
        return false;
    }

    public bool RemoveUsers()
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            try
            {
                foreach (var sql in RepositoryHelper.DeleteAllRecentUsers)
                {
                    connection.ExecuteAsync(sql);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Remove all recent users");
                return false;
            }
        }
        _logger.LogError("Could not access db connection");

        return false;
    }


    public bool RemoveUser(string id)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            try
            {
                foreach (var sql in RepositoryHelper.DeleteRecentUser)
                {
                    connection.ExecuteAsync(sql);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Remove all recent users");
                return false;
            }
        }
        _logger.LogError("Could not access db connection");

        return false;
    }
}
