using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Services;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.AlbumSearch;

public class AddRecentPlaylistTask(IPlaylistViewModelProvider _playlistProvider,
                                   IDispatcher _dispatcher,
                                   ISearchItemRepository _searchRepo)
            : IAddRecentPlaylistTask
{
    public void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, Playlist model)
    {
        Task.Run(() => RunTask(viewModel, model));
    }

    private void RunTask(IRecentViewModel<IPlaylistViewModel> viewModel, Playlist model)
    {
        if (_searchRepo.AddPlaylist(model))
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Add(_playlistProvider.CreatePlaylist(model));
                viewModel.RefreshView();
            });
        }
    }
}
