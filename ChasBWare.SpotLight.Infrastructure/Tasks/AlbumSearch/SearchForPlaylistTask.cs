using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Tasks.Base;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.AlbumSearch;

public class SearchForPlaylistTask(IPlaylistViewModelProvider playlistProvider,
                                   IDispatcher dispatcher,
                                   ISpotifyPlaylistRepository _playlistRepository)
           : BasePlaylistLoaderTask(playlistProvider, dispatcher),
             ISearchForPlaylistTask
{
    public void Execute(ISearchPlaylistsViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(ISearchPlaylistsViewModel viewModel)
    {
        if (string.IsNullOrWhiteSpace(viewModel.SearchText))
        {
            return;
        }

        var items = _playlistRepository.SearchForPlaylists(viewModel.SearchText);
        _dispatcher.Dispatch(() =>
        {
            AddItems(viewModel, items);
            viewModel.IsPopupOpen = viewModel.Items.Count > 0;
        });
    }
}

