using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;

public interface IArtistAlbumsLoaderTask
{
    void Execute(IArtistViewModel viewModel);
}


