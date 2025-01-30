using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface ILoadAvailableDevicesTask 
    {
        void Execute(IDeviceListViewModel viewModel);
    }
}
