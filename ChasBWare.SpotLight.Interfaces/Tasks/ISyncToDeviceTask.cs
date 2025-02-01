using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface ISyncToDeviceTask
    {
        void Execute(IPlayerControlViewModel viewModel);
    }

}
