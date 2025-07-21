using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public static class PlaylistViewModelHelper 
{


    public static void ShowPlayingTrack(this IPlaylistViewModel playlist, string? trackId, TrackStatus status)
    {
        if (trackId != null && playlist.TracksViewModel.LoadStatus == LoadState.Loaded)
        {
            var track = playlist.TracksViewModel.Items.FirstOrDefault(t => t.Id == trackId);
            if (track != null)
            {
                track.Status = status;
            }
        }
    }
}
