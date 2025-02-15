using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Tasks.Base;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.AlbumSearch;

public class SearchForPlaylistTask(IServiceProvider serviceProvider,
                                   IDispatcher dispatcher,
                                   ILibraryViewModel library,
                                   ISpotifyPlaylistRepository _playlistRepository)
           : BasePlaylistLoaderTask(serviceProvider, dispatcher, library),
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

        var items = _playlistRepository.FindPlaylists(viewModel.SearchText);
        AddItems(viewModel, items);
        _dispatcher.Dispatch(() =>
        {
             viewModel.IsPopupOpen = viewModel.Items.Count > 0;
        });
    }
}

