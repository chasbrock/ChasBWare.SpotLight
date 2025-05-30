using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Library;

public interface ILibraryLoaderTask
{
    void Load(ILibraryViewModel viewModel);
    void Refresh(ILibraryViewModel viewModel);
}

