using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;

namespace ChasBWare.SpotLight.Spotify.Repositories;

public class SpotifyTrackRepository(ISpotifyActionManager _actionManager)
           : ISpotifyTrackRepository
{
    public List<Track> GetPlaylistTracks(string playlistId, PlaylistType playlistType)
    {
        if (_actionManager.Status != ConnectionStatus.Connected)
        {
            return [];
        }

        switch (playlistType)
        {
            case PlaylistType.Playlist:
                {
                    var fullTracks = _actionManager.GetPlaylistTracks(playlistId);
                    if (fullTracks != null)
                    {
                        return fullTracks.Select(ft => ft.CopyToTrack()).ToList();
                    }

                    break;
                }

            case PlaylistType.Album:
                {
                    var simpleTracks = _actionManager.GetAlbumTracks(playlistId);
                    if (simpleTracks != null)
                    {
                        return simpleTracks.Select(ft => ft.CopyToTrack()).OrderBy(t => t.TrackNumber).ToList();
                    }

                    break;
                }
        }
        return [];
    }
}
