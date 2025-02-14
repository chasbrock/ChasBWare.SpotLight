using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Repositories
{
    public class SpotifyPlaylistRepository(ISpotifyActionManager _actionManager)
               : ISpotifyPlaylistRepository
    {
        public List<Playlist> FindAlbums(string searchText)
        {
            var simpleAlbums = _actionManager.FindAlbums(searchText);
            return simpleAlbums.Select(sa => sa.CopyToPlaylist()).ToList();
        }


        public List<Playlist> GetPlaylists(PlaylistType playlistType)
        {
            return playlistType switch
            {
                PlaylistType.Album => GetCurrentUsersAlbums(),
                PlaylistType.Playlist => GetCurrentUsersPlaylists(),
                _ => [],
            };
        }

        private List<Playlist> GetCurrentUsersAlbums()
        {
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
}
