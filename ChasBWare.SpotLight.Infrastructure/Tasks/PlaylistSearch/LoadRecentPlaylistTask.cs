using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.PlaylistSearch;

public class LoadRecentPlaylistTask(IServiceProvider _serviceProvider,
                                    IDispatcher _dispatcher,
                                    ISearchItemRepository _searchRepo,
                                    ILibraryViewModel _library)
           : ILoadRecentPlaylistTask
{
    public  void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, PlaylistType playlistType)
    {
        Task.Run(() => RunTask(viewModel, playlistType));
    }

    private void RunTask(IRecentViewModel<IPlaylistViewModel> viewModel, PlaylistType playlistType)
    {
        var items = _searchRepo.GetPlaylists(playlistType);
        if (items.Count != 0)
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();

                foreach (var item in items)
                {
                    var playlistViewModel = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
                    playlistViewModel.Model = item;
                    playlistViewModel.LastAccessed = item.LastAccessed;
                    playlistViewModel.InLibrary = _library.Exists(playlistViewModel.Id);
                    viewModel.Items.Add(playlistViewModel);
                }

                viewModel.LoadStatus = LoadState.Loaded;
            });
        }
    }
}
