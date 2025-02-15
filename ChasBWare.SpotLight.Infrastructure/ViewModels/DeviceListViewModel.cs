using System.Collections.ObjectModel;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;
using ChasBWare.SpotLight.Infrastructure.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public partial class DeviceListViewModel
                       : Notifyable, 
                         IDeviceListViewModel,
                         INavigationClient
    {
        private IDeviceViewModel? _selectedDevice;
        private IServiceProvider _serviceProvider;
        private INavigator _navigator;
        private IMessageService<ActiveDeviceChangedMessage> _activeDeviceMessageService;
        private Uri? _lastCaller;
        public DeviceListViewModel(IServiceProvider serviceProvider,
                                   INavigator navigator,
                                   IPlayerControlViewModel playerControlViewModel,
                                   IMessageService<ActiveDeviceChangedMessage> activeDeviceMessageService)
        {
            _serviceProvider = serviceProvider;
            _activeDeviceMessageService = activeDeviceMessageService;
            _navigator = navigator;
            _navigator.RegisterOnNavigate(this);
            PlayerControlViewModel = playerControlViewModel;
            RefreshCommand = new Command(() => Refresh());
            Refresh();
        }

        public PageType PageType { get; } = PageType.Devices;
        public IPlayerControlViewModel PlayerControlViewModel { get; }

        public ICommand RefreshCommand { get; }

        public void OnNavigationRecieved(Uri? callerPath) 
        {
            _lastCaller = callerPath;
            Refresh();
        }

        public void Refresh() 
        {
            var task = _serviceProvider.GetService<ILoadAvailableDevicesTask>();
            task?.Execute(this);
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

                    var selectedDevice = _selectedDevice ?? new DeviceViewModel();
                    _activeDeviceMessageService.SendMessage(new ActiveDeviceChangedMessage(selectedDevice.Model));
                }
            }
        }
    }
}
