using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface IQueueLoaderTask
    { 
        void Execute(IQueueViewModel viewModel);
    }
}
