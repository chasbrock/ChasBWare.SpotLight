using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class TrackPopupViewModel(IPopupService popupService,
                                        IPopupItemService _popupItemService)
                   : PopupMenuViewModel(popupService)
{
    public void SetTrack(IPlaylistViewModel? playlist, ITrackViewModel? track)
    {
        MenuGroups.Clear();
        if (track != null)
        {
            _popupItemService.AddMenuItem(this, track, PopupActivity.Play);
            _popupItemService.AddMenuItem(this, track, PopupActivity.Copy);
        }

        if (playlist != null)
        {
            _popupItemService.AddMenuItem(this, playlist, PopupActivity.Play);
            _popupItemService.AddMenuItem(this, playlist, PopupActivity.Save);
            _popupItemService.AddMenuItem(this, playlist, PopupActivity.Copy);
        }

        if (track != null)
        {
            _popupItemService.AddMenuItem(this, track, PopupActivity.Hate);
        }

        RecalcSize();
    }
}

