using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface ILoadRecentArtistTask
    {
        void Execute(IRecentArtistsViewModel viewModel);
    }

}
