using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Repositories;

public class SpotifyPlaylistRepository(ISpotifyActionManager _actionManager)
           : ISpotifyPlaylistRepository
{

    public Playlist? FindPlaylist(string playlistId, PlaylistType playlistType)
    {
        if (!_actionManager.Status.IsActiveState()) 
        {
            return null;
        }

        if (playlistType == PlaylistType.Album)
        {
            var fullAlbum = _actionManager.FindAlbum(playlistId);
            if (fullAlbum != null)
            {
                return fullAlbum.CopyToPlaylist();
            }
        }
        else
        {
            var fullPlaylist = _actionManager.FindPlaylist(playlistId);
            if (fullPlaylist != null)
            {
                return fullPlaylist.CopyToPlaylist();
            }
        }
        return null;
    }
    
    public List<Playlist> SearchForAlbums(string searchText)
    {
        if (!_actionManager.Status.IsActiveState())
        {
            return [];
        }

        var simpleAlbums = _actionManager.SearchForAlbums(searchText);
        if (simpleAlbums != null)
        {
            return simpleAlbums.Select(sa => sa.CopyToPlaylist()).ToList();
        }
        return [];
    }

    public List<Playlist> SearchForPlaylists(string searchText)
    {
        if (!_actionManager.Status.IsActiveState())
        {
            return [];
        }

        var fullPlaylists = _actionManager.SearchForPlaylists(searchText);
        List<Playlist> items = [];
        if (fullPlaylists != null)
        {
            foreach (var fullPlaylist in fullPlaylists)
            {
                var playlist = fullPlaylist.CopyToPlaylist();
                if (playlist != null)
                {
                    items.Add(playlist);
                }
            }
        }

        return items;
    }

    public List<Playlist> GetPlaylists(PlaylistType playlistType)
    {
        if (!_actionManager.Status.IsActiveState())
        {
            return [];
        }

        return playlistType switch
        {
            PlaylistType.Album => GetCurrentUsersAlbums(),
            PlaylistType.Playlist => GetCurrentUsersPlaylists(),
            _ => [],
        };
    }

    public bool SetPlaylistSaveStatus(string playlistId, PlaylistType playlistType, bool save)
    {
        if (!_actionManager.Status.IsActiveState())
        {
            return false;
        }

        switch (playlistType)
        {
            case PlaylistType.Album:
                return _actionManager.SetAlbumSaveStatus(playlistId, save);
            case PlaylistType.Playlist:
                return _actionManager.SetPlaylistSaveStatus(playlistId, save);
            default:
                return true;
        }
    }

    private List<Playlist> GetCurrentUsersAlbums()
    {
        if (!_actionManager.Status.IsActiveState())
        {
            return [];
        }

        List<Playlist> playlists = [];
        var savedAlbums =  _actionManager.GetCurrentUsersAlbums();
        if (savedAlbums != null)
        {
            // push last accessed date to way back so the
            // the recently used it more reflective
            var lastAccessed = DateTime.Today.AddYears(-1);
            foreach (var savedAlbum in savedAlbums)
            {
                var playlist = savedAlbum.CopyToPlaylist();
                if (playlist != null)
                {
                    playlist.LastAccessed = lastAccessed;
                    playlists.Add(playlist);
                }
            }
        }
        return playlists; 
    }

    private List<Playlist> GetCurrentUsersPlaylists()
    {
        if (!_actionManager.Status.IsActiveState())
        {
            return [];
        }

        List<Playlist> playlists = [];
        var fullPlaylists =  _actionManager.GetCurrentUsersPlaylists();
        if (fullPlaylists != null)
        {
            // push last accessed date to way back so the
            // the recently used it more reflective
            var lastAccessed = DateTime.Today.AddYears(-1);
            foreach (var fullPlaylist in fullPlaylists)
            {
                var playlist = fullPlaylist.CopyToPlaylist();
                if (playlist != null)
                {
                    playlist.LastAccessed = lastAccessed;
                    playlists.Add(playlist);
                }
            }
        }
        return playlists;
    }

}
