using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface IPlaylistRepository
    {
        Task<List<Tuple<Playlist, DateTime>>> GetPlaylists(string? userId, PlaylistType playlistType, bool isSaved);
        Task<bool> RemoveSavedItem(string userId, string playlistId);
        Task<int> UpdateLastAccessed(string userId, string playlistId, DateTime lastAccessed);
    }
}