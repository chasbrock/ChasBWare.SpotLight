using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.ArtistSearch;

public class SearchForArtistTask(IDispatcher _dispatcher,
                                 ISpotifyArtistRepository _artistRepository)
     : ISearchForArtistTask
{
    public void Execute(ISearchArtistsViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(ISearchArtistsViewModel viewModel)
    {
        if (string.IsNullOrWhiteSpace(viewModel.SearchText))
        {
            return;
        }

        var items = _artistRepository.SearchForArtists(viewModel.SearchText);
        _dispatcher.Dispatch(() =>
        {
            viewModel.Items.Clear();
            foreach (var item in items)
            {
                viewModel.Items.Add(item);
            }

            viewModel.IsPopupOpen = viewModel.Items.Count > 0;
            viewModel.RefreshView();
        });
    }
}

