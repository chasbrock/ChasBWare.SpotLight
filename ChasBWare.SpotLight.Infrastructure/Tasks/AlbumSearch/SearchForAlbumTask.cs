using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Tasks.Base;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.AlbumSearch;

public class SearchForAlbumTask(IPlaylistViewModelProvider playlistProvider,
                                IDispatcher dispatcher,
                                ISpotifyPlaylistRepository _spotifyPlaylistRepo)
           : BasePlaylistLoaderTask(playlistProvider, dispatcher),
             ISearchForAlbumTask
{

    public void Execute(ISearchAlbumsViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(ISearchAlbumsViewModel viewModel)
    {
        if (string.IsNullOrWhiteSpace(viewModel.SearchText))
        {
            return;
        }

        var items = _spotifyPlaylistRepo.SearchForAlbums(viewModel.SearchText);

        _dispatcher.Dispatch(() =>
         {
             AddItems(viewModel, items);
             viewModel.IsPopupOpen = viewModel.Items.Count > 0;
         });
    }
}

