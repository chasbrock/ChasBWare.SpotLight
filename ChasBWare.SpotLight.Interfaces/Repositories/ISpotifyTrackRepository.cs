using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyTrackRepository 
    {
        Task<List<Track>> GetPlaylistTracks(string playlistId, PlaylistType playlistType);
    }
}