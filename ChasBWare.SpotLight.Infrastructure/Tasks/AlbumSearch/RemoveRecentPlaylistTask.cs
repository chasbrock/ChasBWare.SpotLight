using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.AlbumSearch;

public class RemoveRecentPlaylistTask(IDispatcher _dispatcher,
                                   ISearchItemRepository _searchRepo)
           : IRemovePlaylistTask
{
    public void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, IPlaylistViewModel item)
    {
        Task.Run(() => RunTask(viewModel, item));
    }

    public void Execute(IRecentViewModel<IPlaylistViewModel> viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }


    private void RunTask(IRecentViewModel<IPlaylistViewModel> viewModel)
    {
        if (_searchRepo.RemovePlaylists(PlaylistType.Album))
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();
                viewModel.SelectedItem = null;
                viewModel.RefreshView();
            });
        }
    }

    private void RunTask(IRecentViewModel<IPlaylistViewModel> viewModel, IPlaylistViewModel item)
    {
        if (_searchRepo.RemovePlaylist(item.Id))
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Remove(item);
                viewModel.SelectedItem = null;
                viewModel.RefreshView();
            });
        }
    }
}
