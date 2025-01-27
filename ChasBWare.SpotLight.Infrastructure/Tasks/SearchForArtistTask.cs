using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class SearchForArtistTask(IDispatcher _dispatcher, ISpotifyArtistRepository _artistRepository)
         : ISearchForArtistTask
    {
        public async void Execute(ISearchArtistsViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.SearchText))
            {
                return;
            }

            var items = await _artistRepository.FindArtists(viewModel.SearchText);
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();
                foreach (var item in items)
                {
                    viewModel.Items.Add(item);
                }

                viewModel.IsPopupOpen = viewModel.Items.Count > 0;
            });
        }
    }
}
