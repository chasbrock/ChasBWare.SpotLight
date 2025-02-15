using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Library;

public interface ITransferToLibraryTask
{
    void Execute(IPlaylistViewModel viewModel, bool save);
}

