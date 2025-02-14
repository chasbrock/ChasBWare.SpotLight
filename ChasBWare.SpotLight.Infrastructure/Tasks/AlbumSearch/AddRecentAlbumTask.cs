using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.AlbumSearch;

public class AddRecentAlbumTask(IServiceProvider _serviceProvider,
                                 IDispatcher _dispatcher,
                                 ISearchItemRepository _searchRepo)
            : IAddRecentAlbumTask
{
    public void Execute(IRecentAlbumsViewModel viewModel, Playlist model)
    {
        Task.Run(() => RunTask(viewModel, model));
    }

    private void RunTask(IRecentAlbumsViewModel viewModel, Playlist model)
    {
        if (_searchRepo.AddPlaylist(model))
        {
            _dispatcher.Dispatch(() =>
            {
                var item = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
                item.Model = model;
                viewModel.Items.Add(item);
                viewModel.RefreshView();
            });
        }
    }
}
