using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface IArtistAlbumsLoaderTask
    {
        void Execute(IArtistViewModel viewModel);
    }
}
