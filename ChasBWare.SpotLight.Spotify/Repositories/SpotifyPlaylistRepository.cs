using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Classes;

namespace ChasBWare.SpotLight.Spotify.Repositories
{
    public class SpotifyPlaylistRepository(SpotifyActionManager _actionManager)
               : ISpotifyPlaylistRepository
    {
        public async Task<List<Tuple<Playlist, DateTime>>> GetPlaylists(PlaylistType playlistType)
        {
            return playlistType switch
            {
                PlaylistType.Album => await GetCurrentUsersAlbums(),
                PlaylistType.Playlist => await GetCurrentUsersPlaylists(),
                _ => [],
            };
        }

        private async Task<List<Tuple<Playlist, DateTime>>> GetCurrentUsersAlbums()
        {
            List<Tuple<Playlist, DateTime>> playlists = [];
            var savedAlbums = await _actionManager.GetCurrentUsersAlbums();
            if (savedAlbums != null)
            {
                foreach (var savedAlbum in savedAlbums)
                {
                    var playlist = savedAlbum.CopyToPlaylist();
                    if (playlist != null)
                    {
                        playlists.Add(new Tuple<Playlist, DateTime>(playlist, DateTime.Now));
                    }
                }
            }
            return playlists; 
        }

        private async Task<List<Tuple<Playlist, DateTime>>> GetCurrentUsersPlaylists()
        {
            List<Tuple<Playlist, DateTime>> playlists = [];
            var fullPlaylists = await _actionManager.GetCurrentUsersPlaylists();
            if (fullPlaylists != null)
            {
                foreach (var fullPlaylist in fullPlaylists)
                {
                    var playlist = fullPlaylist.CopyToPlaylist();
                    if (playlist != null)
                    {
                        playlists.Add(new Tuple<Playlist, DateTime>(playlist, DateTime.Now));
                    }
                }
            }
            return playlists;
        }
    }
}
