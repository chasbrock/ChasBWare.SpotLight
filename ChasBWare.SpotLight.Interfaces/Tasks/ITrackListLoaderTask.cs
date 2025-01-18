using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface ITrackListLoaderTask
    {
        public void Execute(IPlaylistViewModel viewModel);
    }
}
