using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;

namespace ChasBWare.SpotLight.Spotify.Repositories
{
    public class SpotifyTrackRepository(ISpotifyActionManager _actionManager)
              : ISpotifyTrackRepository
    {
        public async Task<List<Track>> GetPlaylistTracks(string playlistId, PlaylistType playlistType)
        {
            var fullTracks = await _actionManager.GetPlaylistTracks(playlistId);
            if (fullTracks != null)
            { 
                return fullTracks.Select(ft => ft.CopyToTrack()).ToList();
            }
            return [];
        }

    
    }
}
