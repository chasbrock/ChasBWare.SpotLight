using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Device
{
    public interface ILoadAvailableDevicesTask 
    {
        void Execute(IDeviceListViewModel viewModel);
    }
}
