using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface IPlaylistRepository
    {
        int AddPlaylists(List<RecentPlaylist> playlists, string userId, bool isSaved);
        List<RecentPlaylist> GetPlaylists(string userId, PlaylistType playlistType, bool isSaved);
        bool RemoveSaved(string userId, string playlistId);
        bool RemoveUnsavedPlaylist(string userId, string artistId);
        bool RemoveUnsavedPlaylists(string userId, PlaylistType playlistType);
    }
}