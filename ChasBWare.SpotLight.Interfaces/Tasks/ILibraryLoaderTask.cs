using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface ILibraryLoaderTask
    {
        void Execute(ILibraryViewModel viewModel);
    }
}
