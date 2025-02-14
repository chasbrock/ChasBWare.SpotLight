using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;

public interface IAddRecentArtistTask
{
    void Execute(IRecentArtistsViewModel viewModel, Artist model);
}
