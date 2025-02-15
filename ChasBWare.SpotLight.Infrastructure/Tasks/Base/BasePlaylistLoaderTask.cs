using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Base;

public class BasePlaylistLoaderTask
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IDispatcher _dispatcher;
    protected readonly ILibraryViewModel _library;

    protected BasePlaylistLoaderTask(IServiceProvider serviceProvider,
                                     IDispatcher dispatcher,
                                     ILibraryViewModel library)
    {
        _serviceProvider = serviceProvider;
        _dispatcher = dispatcher;
        _library = library;
    }

    protected void AddItems(IListViewModel<IPlaylistViewModel> viewModel, ICollection<Playlist> items)
    {
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

