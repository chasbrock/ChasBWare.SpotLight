using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;

namespace ChasBWare.SpotLight.Spotify.Repositories;

public class SpotifyTrackRepository(ISpotifyActionManager _actionManager)
           : ISpotifyTrackRepository
{
    public List<Track> GetPlaylistTracks(Playlist model)
    {
        if (model.Id == null) 
        {
            return [];
        }

        switch (model.PlaylistType)
        {
            case PlaylistType.Playlist:
                {
                    var fullTracks = _actionManager.GetPlaylistTracks(model.Id);
                    if (fullTracks != null)
                    {
                        return fullTracks.Select(ft => ft.CopyToTrack()).ToList();
                    }

                    break;
                }

            case PlaylistType.Album:
                {
                    var simpleTracks = _actionManager.GetAlbumTracks(model.Id);
                    if (simpleTracks != null)
                    {
                        return simpleTracks.Select(ft => ft.CopyToTrack()).OrderBy(t => t.TrackNumber).ToList();
                    }

                    break;
                }

            case PlaylistType.TopTracks:
                {
                    var keyValue = model.Owner.UnpackOwners().FirstOrDefault();
                    if (keyValue != null)
                    {
                        var fullTracks = _actionManager.GetArtistTopTracks(keyValue.Value);
                        if (fullTracks != null)
                        {
                            return fullTracks.Select(ft => ft.CopyToTrack()).OrderBy(t => t.TrackNumber).ToList();
                        }
                    }
                    break;
                }
        }
        return [];
    }
}
