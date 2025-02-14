using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks.Queue
{
    public interface IQueueLoaderTask
    { 
        void Execute(IQueueViewModel viewModel);
    }
}
