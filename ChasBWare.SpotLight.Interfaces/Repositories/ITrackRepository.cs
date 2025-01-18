using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ITrackRepository
    {
        Task<int> AddTracksToPlaylist(string playListId, IEnumerable<Track> tracks);
        Task<List<Track>> GetPlaylistTracks(string playlistId);
    }
}