using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface IGetActiveDeviceTask
    {
        void Execute(IPlayerControlViewModel playerControlViewModel);
    }
}
