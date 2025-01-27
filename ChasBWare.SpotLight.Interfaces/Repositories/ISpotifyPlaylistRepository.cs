using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyPlaylistRepository 
    {
        Task<List<RecentPlaylist>> FindAlbums(string searchText);
        Task<List<RecentPlaylist>> GetPlaylists(PlaylistType playlistType);
    }
}