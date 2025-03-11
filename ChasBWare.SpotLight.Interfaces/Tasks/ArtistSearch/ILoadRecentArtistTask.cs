using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;

public interface ILoadRecentArtistTask
{
    void Execute(IRecentArtistsViewModel viewModel);
}
