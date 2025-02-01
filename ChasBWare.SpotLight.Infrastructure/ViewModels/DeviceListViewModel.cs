using System.Collections.ObjectModel;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;

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
                                   IMessageService<ActiveDeviceChangedMessage> activeDeviceMessageService)
        {
            _serviceProvider = serviceProvider;
            _activeDeviceMessageService = activeDeviceMessageService;
            _navigator = navigator;
            _navigator.RegisterOnNavigate(this);
            RefreshCommand = new Command(() => Refresh());
            Refresh();
        }

        public string Path { get; } = PageType.Devices;

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

                    var selectedDevice = _selectedDevice ?? new DeviceViewModel();
                    _activeDeviceMessageService.SendMessage(new ActiveDeviceChangedMessage(selectedDevice.Model));
                }
            }
        }
    }
}
