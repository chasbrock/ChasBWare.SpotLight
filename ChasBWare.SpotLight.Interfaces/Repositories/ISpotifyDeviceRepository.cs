using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyDeviceRepository
    {
        Task<CurrentContext?> GetCurrentContext();
        Task<List<IDeviceViewModel>> GetAvailableDevices();
        void SetDeviceVolume(int volumePercent);
    }

}