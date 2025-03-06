using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Base;

public class BasePlaylistLoaderTask
{
    protected readonly IPlaylistViewModelProvider _playlistProvider;
    protected readonly IDispatcher _dispatcher;

    protected BasePlaylistLoaderTask(IPlaylistViewModelProvider playlistProvider,
                                     IDispatcher dispatcher)
    {
        _playlistProvider = playlistProvider;
        _dispatcher = dispatcher;
    }

    protected void AddItems(IListViewModel<IPlaylistViewModel> viewModel, List<Playlist> items)
    {
        viewModel.Items.Clear();
        items.ForEach(i => viewModel.Items.Add(_playlistProvider.CreatePlaylist(i)));
        viewModel.LoadStatus = LoadState.Loaded;
        viewModel.RefreshView();
    }
}

