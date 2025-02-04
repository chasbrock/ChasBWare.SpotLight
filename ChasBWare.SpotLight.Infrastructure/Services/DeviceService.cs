using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Models;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

namespace ChasBWare.SpotLight.Infrastructure.Services;

public class DeviceService(IServiceProvider _serviceProvider)
          : IDeviceService
{
    public void SetVolume(int volumePercent)
    {
        var repo = _serviceProvider.GetService<ISpotifyDeviceRepository>();
        if (repo != null)
        {
            Task.Run(() => repo.SetDeviceVolume(volumePercent));
        }
    }
}
