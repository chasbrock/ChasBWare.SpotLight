using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;

namespace ChasBWare.SpotLight.Spotify.Repositories
{
    public class SpotifyPlaylistRepository(ISpotifyActionManager _actionManager)
               : ISpotifyPlaylistRepository
    {
        public async Task<List<RecentPlaylist>> GetPlaylists(PlaylistType playlistType)
        {
            return playlistType switch
            {
                PlaylistType.Album => await GetCurrentUsersAlbums(),
                PlaylistType.Playlist => await GetCurrentUsersPlaylists(),
                _ => [],
            };
        }

        private async Task<List<RecentPlaylist>> GetCurrentUsersAlbums()
        {
            List<RecentPlaylist> playlists = [];
            var savedAlbums = await _actionManager.GetCurrentUsersAlbums();
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

        private async Task<List<RecentPlaylist>> GetCurrentUsersPlaylists()
        {
            List<RecentPlaylist> playlists = [];
            var fullPlaylists = await _actionManager.GetCurrentUsersPlaylists();
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
