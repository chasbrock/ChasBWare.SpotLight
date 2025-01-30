using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface IRemoveArtistTask
    {
        void Execute(IRecentArtistsViewModel viewModel, string userId);
    }

}
