using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class LoadAvailableDevicesTask(IDispatcher _dispatcher,
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
