using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Device
{
    public interface IChangeActiveDeviceTask
    {
        void Execute(IDeviceViewModel selectedDevice);
    }
}
