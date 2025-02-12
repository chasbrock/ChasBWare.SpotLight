using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyDeviceRepository
    {
        CurrentContext? GetCurrentContext();
        List<IDeviceViewModel> GetAvailableDevices();
        void SetDeviceVolume(int volumePercent);

        bool SetDeviceAsActive(string deviceId);
    }
}