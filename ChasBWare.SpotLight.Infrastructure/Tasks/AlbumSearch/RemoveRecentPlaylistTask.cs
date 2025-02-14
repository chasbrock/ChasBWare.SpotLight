using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Repositories;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.AlbumSearch;

public class RemoveRecentPlaylistTask(IDispatcher _dispatcher,
                                   ISearchItemRepository _searchRepo)
           : IRemovePlaylistTask
{
    public void Execute(IRecentAlbumsViewModel viewModel, IPlaylistViewModel item)
    {
        Task.Run(() => RunTask(viewModel, item));
    }

    public void Execute(IRecentAlbumsViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    public void Execute(IRecentPlaylistsViewModel viewModel, IPlaylistViewModel item)
    {
        Task.Run(() => RunTask(viewModel, item));
    }

    public void Execute(IRecentPlaylistsViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(IRecentAlbumsViewModel viewModel)
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

    private void RunTask(IRecentAlbumsViewModel viewModel, IPlaylistViewModel item)
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

    private void RunTask(IRecentPlaylistsViewModel viewModel)
    {
        if (_searchRepo.RemovePlaylists(PlaylistType.Playlist))
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();
                viewModel.SelectedItem = null;
                viewModel.RefreshView();
            });
        }
    }

    private void RunTask(IRecentPlaylistsViewModel viewModel, IPlaylistViewModel item)
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
