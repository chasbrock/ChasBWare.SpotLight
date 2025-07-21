using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class DevicePopupViewModel(IPopupService popupService,
                                          IServiceProvider _serviceProvider)
                    : PopupMenuViewModel(popupService)
{
    public const string DeviceName = "DeviceName";

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        object? obj = null;
        if ((query.TryGetValue(DeviceName, out obj) && obj is IDeviceListViewModel device))
        {
            MenuGroups.Clear();

            AddItem(PopupGroup.Devices,
                    PopupActivity.Refresh,
                    caption: "Refresh list",
                    toolTip: "Check to see if active devices has changed",
                    action: (t) =>
                    {
                        var task = _serviceProvider.GetRequiredService<ILoadAvailableDevicesTask>();
                        task.Execute(device);
                        Close();
                    });
            AddItem(PopupGroup.Settings,
                    PopupActivity.Clear,
                    caption: $"Reset Spotify",
                    toolTip: "Reset Spotify connection settings",
                    action: (t) =>
                    {
                        //    ClearSettings();
                    });
            RecalcSize();
        }
    }

/*  private void ClearSettings()
    {
        //  var session = _serviceProvider.GetRequiredService<ISpotifyPlayerController>();
    }
*/
    /// <summary>
    /// correctly builds params for show popup call
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static IDictionary<string, object> BuildParams(IDeviceListViewModel device)
    {
        return new Dictionary<string, object>() { { DeviceName, device } };
    }
}

