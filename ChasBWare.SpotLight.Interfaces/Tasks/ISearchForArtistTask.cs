using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface ISearchForArtistTask
    {
        void Execute(ISearchArtistsViewModel viewModel);
    }

}
