using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.AlbumSearch;

public class AddRecentPlaylistTask(IServiceProvider _serviceProvider,
                                   IDispatcher _dispatcher,
                                   ISearchItemRepository _searchRepo,
                                   ILibraryViewModel _library)
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
                var item = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
                item.Model = model;
                item.InLibrary = _library.Exists(model.Id);
                viewModel.Items.Add(item);
                viewModel.RefreshView();
            });
        }
    }
}
