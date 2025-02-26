using System.Collections.ObjectModel;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using ChasBWare.SpotLight.Infrastructure.ViewModels;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public partial class DeviceListViewModel
                   : Notifyable, 
                     IDeviceListViewModel,
                     INavigationClient
{
    private IDeviceViewModel? _selectedDevice;
    private IServiceProvider _serviceProvider;
    private INavigator _navigator;
    private IMessageService<ActiveItemChangedMessage> _activeDeviceMessageService;
    private Uri? _lastCaller;

    public DeviceListViewModel(IPopupService popupService,
                               IServiceProvider serviceProvider,
                               INavigator navigator,
                               IPlayerControlViewModel playerControlViewModel,
                               IMessageService<ActiveItemChangedMessage> activeItemChangedMessageService,
                               IMessageService<ConnectionStatusChangedMessage> connectionStatusService)
    {
        _serviceProvider = serviceProvider;
        _activeDeviceMessageService = activeItemChangedMessageService;
        _navigator = navigator;
        _navigator.RegisterOnNavigate(this);

        PlayerControlViewModel = playerControlViewModel;
        OpenPopupCommand = new Command<ITrackViewModel>(t => popupService.ShowPopup<DevicePopupViewModel>());

        connectionStatusService.Register(m => Refresh());

        Refresh();
    }

    public PageType PageType { get; } = PageType.Devices;
    public IPlayerControlViewModel PlayerControlViewModel { get; }
    public ICommand OpenPopupCommand { get; }

    public void OnNavigationRecieved(Uri? callerPath) 
    {
        _lastCaller = callerPath;
        Refresh();
    }

    public Continue Refresh() 
    {
        var task = _serviceProvider.GetRequiredService<ILoadAvailableDevicesTask>();
        task.Execute(this);
        return Continue.Yes;
    }

    public ObservableCollection<IDeviceViewModel> Devices { get; } = [];

    public IDeviceViewModel? SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            if (SetField(ref _selectedDevice, value))
            {
                if (_lastCaller != null)
                {
                    _navigator.NavigateTo(_lastCaller);
                    _lastCaller = null;
                }

                if (_selectedDevice != null && ! _selectedDevice.IsActive)
                {
                    var task = _serviceProvider.GetRequiredService<IChangeActiveDeviceTask>();
                    task.Execute(_selectedDevice);
                }

                _activeDeviceMessageService.SendMessage(new ActiveItemChangedMessage(PageType.Devices,  _selectedDevice?.Model));
            }
        }
    }
}
