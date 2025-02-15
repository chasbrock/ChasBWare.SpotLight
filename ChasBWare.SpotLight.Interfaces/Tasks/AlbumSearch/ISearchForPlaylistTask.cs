using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch
{
    public interface ISearchForPlaylistTask
    {
        void Execute(ISearchPlaylistsViewModel viewModel);
    }

}
