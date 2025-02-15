using System;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Tasks.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Dispatching;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.AlbumSearch;

public class SearchForAlbumTask(IServiceProvider serviceProvider, 
                                IDispatcher dispatcher,
                                ILibraryViewModel library,
                                ISpotifyPlaylistRepository _playlistRepository)
           : BasePlaylistLoaderTask(serviceProvider, dispatcher, library),
           ISearchForAlbumTask
    {
    public void Execute(ISearchAlbumsViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private  void RunTask(ISearchAlbumsViewModel viewModel)
    {
        if (string.IsNullOrWhiteSpace(viewModel.SearchText))
        {
            return;
        }

        var items = _playlistRepository.FindAlbums(viewModel.SearchText);

        AddItems(viewModel, items);
        _dispatcher.Dispatch(() =>
        {
            viewModel.IsPopupOpen = viewModel.Items.Count > 0;
        });
    }
}

