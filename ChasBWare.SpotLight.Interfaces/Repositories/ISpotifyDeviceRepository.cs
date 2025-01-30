using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyDeviceRepository
    {
        Task<IDeviceViewModel?> GetActiveDevice();
        Task<List<IDeviceViewModel>> GetAvailableDevices();
    }

}