using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;
using Microsoft.Maui.Controls;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class LoadAvailableDevicesTask(IServiceProvider _serviceporovider, 
                                          IDispatcher _dispatcher,
                                          ISpotifyDeviceRepository _deviceRepository,
                                          IMessageService<ActiveDeviceChangedMessage> _activeDeviceMessageService)
               : ILoadAvailableDevicesTask
    {
        public void Execute(IDeviceListViewModel viewModel)
        {
            Task.Run(() => RunTask(viewModel));
        }

        private async void RunTask(IDeviceListViewModel viewModel)
        {
            var devices = await _deviceRepository.GetAvailableDevices();
            _dispatcher.Dispatch(() =>
            {
                viewModel.Devices.Clear();
                IDeviceViewModel? activeDevice = null;

                if (devices.Count == 0)
                {
                    var deviceViewModel = _serviceporovider.GetRequiredService<IDeviceViewModel>();
                    deviceViewModel.Model = DeviceHelper.GetLocalDevice();
                    viewModel.Devices.Add(deviceViewModel);
                    return;
                }

                foreach (var device in devices)
                {
                    viewModel.Devices.Add(device);
                    if (device.IsActive && activeDevice == null)
                    {
                        activeDevice = device;
                    }
                }

                //signal that we have found an active device
                if (activeDevice != null)
                {
                    _activeDeviceMessageService.SendMessage(new ActiveDeviceChangedMessage(activeDevice.Model));
                }
            });
        }
    }
}
