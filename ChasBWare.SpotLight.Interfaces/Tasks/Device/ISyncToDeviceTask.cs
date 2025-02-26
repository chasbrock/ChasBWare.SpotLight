using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks.Device
{
    public interface ISyncToDeviceTask
    {
        void Execute(IPlayerControlViewModel viewModel);
    }
}
