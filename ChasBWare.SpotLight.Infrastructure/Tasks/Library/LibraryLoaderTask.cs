using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Library;

public class LibraryLoaderTask(IDispatcher _dispatcher,
                               ILibraryRepository _libraryRepo,
                               ISpotifyPlaylistRepository _spotifyPlaylistRepo)
           : ILibraryLoaderTask
{
    public void Load(ILibraryViewModel viewModel)
    {
        if (viewModel.LoadStatus != LoadState.NotLoaded)
        {
            return;
        }
        Task.Run(() => RunLoad(viewModel));
    }

    public void Refresh(ILibraryViewModel viewModel)
    {
        if (viewModel.LoadStatus != LoadState.Loaded)
        {
            return;
        }
        Task.Run(() => RunRefresh(viewModel));
    }

    private void RunLoad(ILibraryViewModel viewModel)
    {
        _dispatcher.Dispatch(() =>
        {
            viewModel.LoadStatus = LoadState.Loading;
            viewModel.Items.Clear();
        });

        bool refreshNeeded = false;
        foreach (var playlistType in Enum.GetValues<PlaylistType>())
        {
            var items = _libraryRepo.GetPlaylists(playlistType);
            refreshNeeded |= items.Count == 0;
            _dispatcher.Dispatch(() =>
             {
                 viewModel.AddItems(items);
             });
        }

        if (refreshNeeded)
        {
            RunRefresh(viewModel);
        }
        else
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.LoadStatus = LoadState.Loaded;
            });
        }
    }

    private void RunRefresh(ILibraryViewModel viewModel)
    {
        viewModel.LoadStatus = LoadState.Loading;
        var ids = _libraryRepo.GetPlaylistIds();

        // get all saved items on spotify account
        List<Playlist> items = [];
        foreach (var playlistType in Enum.GetValues<PlaylistType>())
        {
            items.AddRange(_spotifyPlaylistRepo.GetPlaylists(playlistType));
        }

        // get list that are not known  locally
        List<Playlist> toAdd = items.Where(pl => pl.Id != null && !ids.Contains(pl.Id)).ToList();
        if (toAdd.Count > 0)
        {
            _libraryRepo.AddPlaylists(toAdd);
            _dispatcher.Dispatch(() =>
            {
                viewModel.AddItems(toAdd);
            });
        }

        // get list of any that are in local library that should be removed
        List<string> toDelete = ids.Where(id => !items.Any(pl => pl.Id == id)).ToList();
        if (toDelete.Count > 0)
        {
            _libraryRepo.RemovePlaylists(toDelete);
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.RemoveAll(vm => toDelete.Contains(vm.Id));
            });
        }

        _dispatcher.Dispatch(() =>
        {
            viewModel.LoadStatus = LoadState.Loaded;
            viewModel.RefreshView();
        });
    }


}
