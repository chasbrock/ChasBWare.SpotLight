using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface ISearchForAlbumTask 
    {
        void Execute(ISearchAlbumsViewModel viewModel);
    }

}
