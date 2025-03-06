using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Device;


public class LoadAvailableDevicesTask(IServiceProvider _serviceProvider, 
                                      IDispatcher _dispatcher,
                                      ILogger<LoadAvailableDevicesTask> _logger,
                                      ISpotifyDeviceRepository _deviceRepository)
           : ILoadAvailableDevicesTask
{
    public void Execute(IDeviceListViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(IDeviceListViewModel viewModel)
    {
         var devices = _deviceRepository.GetAvailableDevices();
       
        _dispatcher.Dispatch(() =>
        {
            try
            {
                viewModel.Devices.Clear();
         
                if (devices.Count == 0)
                {
                    var deviceViewModel = _serviceProvider.GetRequiredService<IDeviceViewModel>();
                    deviceViewModel.Model = DeviceHelper.GetLocalDevice();
                    viewModel.Devices.Add(deviceViewModel);
                    return;
                }

                foreach (var device in devices)
                {
                    viewModel.Devices.Add(device);
                }
                viewModel.SelectedDevice = devices.FirstOrDefault(d => d.IsActive);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed to update ui");
            }
        });
    }
}
