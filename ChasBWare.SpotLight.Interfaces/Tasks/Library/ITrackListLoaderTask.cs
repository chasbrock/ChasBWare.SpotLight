using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks.Library
{
    public interface ITrackListLoaderTask
    {
        public void Execute(IPlaylistViewModel viewModel);
    }
}
