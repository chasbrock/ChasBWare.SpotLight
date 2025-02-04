using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks;

public interface ISetPlaylistSavedStatus
{
    void Execute(IPlaylistViewModel viewModel, bool save);
}

