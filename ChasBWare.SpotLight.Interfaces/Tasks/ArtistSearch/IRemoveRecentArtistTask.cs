using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;

public interface IRemoveRecentArtistTask
{
    void Execute(IRecentArtistsViewModel viewModel, IArtistViewModel item);
    void Execute(IRecentArtistsViewModel viewModel);
}
