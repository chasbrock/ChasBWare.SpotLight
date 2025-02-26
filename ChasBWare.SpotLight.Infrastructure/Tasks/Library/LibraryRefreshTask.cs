using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Library;

public class LibraryRefreshTask(IServiceProvider _serviceProvider,
                               IDispatcher _dispatcher,
                               ILibraryRepository _libraryRepo,
                               ISpotifyPlaylistRepository _spotifyPlaylistRepo)
           : ILibraryRefreshTask
{
    public void Execute(ILibraryViewModel viewModel)
    {
        if (viewModel.LoadStatus != LoadState.Loaded)
        {
            return;
        }
        Task.Run(() => RunTask(viewModel));
    }


    private void RunTask(ILibraryViewModel viewModel)
    {
        viewModel.LoadStatus = LoadState.Loading;
        var ids = _libraryRepo.GetPlaylistIds();

        foreach (var playlistType in Enum.GetValues<PlaylistType>())
        {
            var playlists = _spotifyPlaylistRepo.GetPlaylists(playlistType)
                                           .Where(pl => pl.Id != null && !ids.Contains(pl.Id))
                                           .ToList();
            if (playlists.Count > 0)
            {
                _libraryRepo.AddPlaylists(playlists);
                AddPlaylistsToModel(viewModel, playlists);
            }
        }
        _dispatcher.Dispatch(() =>
        {
            viewModel.LoadStatus = LoadState.Loaded;
        });
    }

    private void AddPlaylistsToModel(ILibraryViewModel viewModel, List<Playlist> items)
    {
        _dispatcher.Dispatch(() =>
        {
            foreach (var item in items)
            {
                var playlistViewModel = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
                playlistViewModel.Model = item;
                playlistViewModel.LastAccessed = item.LastAccessed;
                playlistViewModel.IsExpanded = false;
                playlistViewModel.InLibrary = true;
                viewModel.Items.Add(playlistViewModel);
            }
        });
    }
}
