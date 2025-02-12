using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ITrackRepository
    {
        int AddTracksToPlaylist(string playListId, IEnumerable<Track> tracks);
        List<Track> GetPlaylistTracks(string playlistId);
    }
}