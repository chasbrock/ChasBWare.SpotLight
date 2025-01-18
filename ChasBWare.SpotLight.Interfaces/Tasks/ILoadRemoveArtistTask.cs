using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface ILoadRemoveArtistTask
    {
        void Execute(IRecentArtistsViewModel viewModel, string userId);
    }

}
