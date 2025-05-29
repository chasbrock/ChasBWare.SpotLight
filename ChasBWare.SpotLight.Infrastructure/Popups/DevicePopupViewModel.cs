using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.ViewModels;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class DevicePopupViewModel(IPopupService popupService,
                                          IServiceProvider _serviceProvider)
                    : PopupMenuViewModel(popupService)
{
    public void SetItem(IDeviceListViewModel viewModel)
    {
        MenuGroups.Clear();

        AddItem(PopupGroup.Devices,
                PopupActivity.Refresh,
                caption: "Refresh list",
                toolTip: "Check to see if active devices has changed",
                action: (t) =>
                {
                    var task = _serviceProvider.GetRequiredService<ILoadAvailableDevicesTask>();
                    task.Execute(viewModel);
                    Close();
                });
        AddItem(PopupGroup.Settings,
                PopupActivity.Clear,
                caption: $"Reset Spotify",
                toolTip: "Reset Spotify connection settings",
                action: (t) =>
                {
                    ClearSettings();
                });
        RecalcSize();
    }

    private void ClearSettings()
    {
        //  var session = _serviceProvider.GetRequiredService<ISpotifyPlayerController>();
    }
}

