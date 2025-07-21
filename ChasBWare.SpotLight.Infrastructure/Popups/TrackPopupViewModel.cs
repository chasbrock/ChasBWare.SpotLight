using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class TrackPopupViewModel(IPopupService popupService,
                                        IPopupItemService _popupItemService)
                   : PopupMenuViewModel(popupService)
{

    public const string PlaylistName = "Playlist";
    public const string TrackName = "Track";

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        MenuGroups.Clear();
        object? obj=null;
        if ((query.TryGetValue(TrackName, out obj) && obj is ITrackViewModel track))
        {
            _popupItemService.AddMenuItem(this, track, PopupActivity.Play);
            _popupItemService.AddMenuItem(this, track, PopupActivity.Copy);
            _popupItemService.AddMenuItem(this, track, PopupActivity.Hate);
        }

        if ((query.TryGetValue(PlaylistName, out obj) && obj is IPlaylistViewModel playlist))
        {
            _popupItemService.AddMenuItem(this, playlist, PopupActivity.Play);
            _popupItemService.AddMenuItem(this, playlist, PopupActivity.Copy);
        }

       // RecalcSize();
    }


    /// <summary>
    /// correctly builds params for show popup call
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static IDictionary<string, object> BuildParams(IPlaylistViewModel? playlist, ITrackViewModel? track)
    {
        var paramList = new Dictionary<string, object>();
        if (playlist != null) 
        {
            paramList.Add(PlaylistName, playlist);
        }

        if (track != null)
        {
            paramList.Add(TrackName, track);
        }

        return paramList;
    }
}

