using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;

public interface ISearchForArtistTask
{
    void Execute(ISearchArtistsViewModel viewModel);
}
