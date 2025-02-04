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
        if (playlist != null)
        { 
            _popupItemService.AddMenuItem(this, playlist, PopupAction.Profile);
            _popupItemService.AddMenuItem(this, playlist, PopupAction.Play);
            _popupItemService.AddMenuItem(this, playlist, PopupAction.AddToQueue);
        }

        if (track != null) 
        {
            _popupItemService.AddMenuItem(this, track, PopupAction.Hate);
            _popupItemService.AddMenuItem(this, track, PopupAction.Play);
            _popupItemService.AddMenuItem(this, track, PopupAction.AddToQueue);
        }
        Height = GetHeight();
    }
}

