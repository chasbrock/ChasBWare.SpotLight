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
        public List<RecentPlaylist> FindAlbums(string searchText)
        {
            var simpleAlbums = _actionManager.FindAlbums(searchText);
            return simpleAlbums.Select(sa => sa.CopyToPlaylist()).ToList();
        }


        public List<RecentPlaylist> GetPlaylists(PlaylistType playlistType)
        {
            return playlistType switch
            {
                PlaylistType.Album => GetCurrentUsersAlbums(),
                PlaylistType.Playlist => GetCurrentUsersPlaylists(),
                _ => [],
            };
        }

        private List<RecentPlaylist> GetCurrentUsersAlbums()
        {
            List<RecentPlaylist> playlists = [];
            var savedAlbums =  _actionManager.GetCurrentUsersAlbums();
            if (savedAlbums != null)
            {
                foreach (var savedAlbum in savedAlbums)
                {
                    var playlist = savedAlbum.CopyToPlaylist();
                    if (playlist != null)
                    {
                        playlist.LastAccessed = DateTime.Now;
                        playlists.Add(playlist);
                    }
                }
            }
            return playlists; 
        }

        private List<RecentPlaylist> GetCurrentUsersPlaylists()
        {
            List<RecentPlaylist> playlists = [];
            var fullPlaylists =  _actionManager.GetCurrentUsersPlaylists();
            if (fullPlaylists != null)
            {
                foreach (var fullPlaylist in fullPlaylists)
                {
                    var playlist = fullPlaylist.CopyToPlaylist();
                    if (playlist != null)
                    {
                        playlist.LastAccessed = DateTime.Now;
                        playlists.Add(playlist);
                    }
                }
            }
            return playlists;
        }
    }
}
