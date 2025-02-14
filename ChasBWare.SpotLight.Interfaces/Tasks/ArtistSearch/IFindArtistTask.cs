using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;

public interface IFindArtistTask
{
    void Execute(IRecentArtistsViewModel viewModel, string artistId);
}
