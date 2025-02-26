using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Library
{
    public interface ILibraryRefreshTask
    {
        void Execute(ILibraryViewModel viewModel);
    }
}
