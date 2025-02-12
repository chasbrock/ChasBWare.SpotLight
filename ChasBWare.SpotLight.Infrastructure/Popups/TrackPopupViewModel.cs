using System.ComponentModel;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Services;
using ChasBWare.SpotLight.Infrastructure.Utility;
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
            _popupItemService.AddMenuItem(this, track, PopupActivity.AddToQueue);
        }

        if (playlist != null)
        { 
            _popupItemService.AddMenuItem(this, playlist, PopupActivity.Play);
            _popupItemService.AddMenuItem(this, playlist, PopupActivity.AddToQueue);
            _popupItemService.AddMenuItem(this, playlist, PopupActivity.Save);
        }

        if (track != null)
        {
            _popupItemService.AddMenuItem(this, track, PopupActivity.Hate);
        }

        RecalcSize();
    }
}

