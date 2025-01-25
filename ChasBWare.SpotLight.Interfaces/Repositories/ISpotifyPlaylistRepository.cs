using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyPlaylistRepository 
    {
        Task<List<RecentPlaylist>> GetPlaylists(PlaylistType playlistType);
    }
}