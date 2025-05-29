using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch
{
    public interface ISearchForAlbumTask
    {
        void Execute(ISearchAlbumsViewModel viewModel);
    }

}
