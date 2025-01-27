using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface IPlaylistRepository
    {
        Task<int> AddPlaylists(List<RecentPlaylist> playlists, string userId, bool isSaved);
        Task<List<RecentPlaylist>> GetPlaylists(string userId, PlaylistType playlistType, bool isSaved);
        Task<bool> RemoveSavedItem(string userId, string playlistId);
    }
}