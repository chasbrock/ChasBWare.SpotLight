using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Tasks.Base;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.PlaylistSearch;

public class LoadRecentPlaylistTask(IServiceProvider serviceProvider,
                                    IDispatcher dispatcher,
                                    ISearchItemRepository _searchRepo,
                                    ILibraryViewModel library)
           : BasePlaylistLoaderTask(serviceProvider, dispatcher, library),
             ILoadRecentPlaylistTask
{
    public void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, PlaylistType playlistType)
    {
        Task.Run(() => RunTask(viewModel, playlistType));
    }

    private void RunTask(IRecentViewModel<IPlaylistViewModel> viewModel, PlaylistType playlistType)
    {
        var items = _searchRepo.GetPlaylists(playlistType);
        AddItems(viewModel, items);
    }

}

