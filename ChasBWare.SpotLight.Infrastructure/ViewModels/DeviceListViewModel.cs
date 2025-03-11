using System.Collections.ObjectModel;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
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
        OpenPopupCommand = new Command<ITrackViewModel>(t => popupService.ShowPopup<DevicePopupViewModel>(onPresenting: vm => vm.SetItem(this)));
        ActivateDeviceCommand = new Command<IDeviceViewModel>(OnActivateDeviceCommand);
        connectionStatusService.Register(OnConnectionStatusChanged);

        Refresh();
    }
    public PageType PageType { get; } = PageType.Devices;
    public IPlayerControlViewModel PlayerControlViewModel { get; }
    public ICommand OpenPopupCommand { get; }
    public ICommand ActivateDeviceCommand { get; }

    public void OnNavigationRecieved(Uri? callerPath) 
    {
        _lastCaller = callerPath;
        if (PlayerControlViewModel.ConnectionStatus.IsActiveState())
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        var task = _serviceProvider.GetRequiredService<ILoadAvailableDevicesTask>();
        task.Execute(this);
    }

    public ObservableCollection<IDeviceViewModel> Devices { get; } = [];

    public IDeviceViewModel? SelectedDevice
    {
        get => _selectedDevice;
        set => SetField(ref _selectedDevice, value);
    }

    private void OnActivateDeviceCommand(IDeviceViewModel device)
    {
        // if the device is already active simply send notificication
        if (device.IsActive)
        {
            _activeDeviceMessageService.SendMessage(new ActiveItemChangedMessage(PageType.Devices, _selectedDevice?.Model));
        }
        else 
        {
            var task = _serviceProvider.GetRequiredService<IChangeActiveDeviceTask>();
            task.Execute(device);
        }

        // return to last active page if we did not change page manually
        if (_lastCaller != null)
        {
            _navigator.NavigateTo(_lastCaller);
            _lastCaller = null;
        }

    }

    private void OnConnectionStatusChanged(ConnectionStatusChangedMessage message)
    {
        if (message.ConnectionStatus.IsActiveState())
        {
             Refresh();
        }
    }

}
