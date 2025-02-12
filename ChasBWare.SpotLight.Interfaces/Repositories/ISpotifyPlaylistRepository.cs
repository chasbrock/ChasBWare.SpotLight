using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyPlaylistRepository 
    {
        List<RecentPlaylist> FindAlbums(string searchText);
        List<RecentPlaylist> GetPlaylists(PlaylistType playlistType);
    }
}