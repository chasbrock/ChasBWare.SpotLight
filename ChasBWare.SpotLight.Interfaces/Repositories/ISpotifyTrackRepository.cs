using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyTrackRepository 
    {
        List<Track> GetPlaylistTracks(Playlist model);
    }
}