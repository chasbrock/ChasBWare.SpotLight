using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface IChangeActiveDeviceTask
    {
        void Execute(IDeviceViewModel selectedDevice);
    }
}
