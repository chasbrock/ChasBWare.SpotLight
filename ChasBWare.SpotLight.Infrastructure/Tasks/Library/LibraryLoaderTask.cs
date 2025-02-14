using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Library;

public class LibraryLoaderTask(IServiceProvider _serviceProvider,
                               IDispatcher _dispatcher,
                               ILibraryRepository _libraryRepo,
                               ISpotifyPlaylistRepository _spotifyPlaylistRepo                                   )
           : ILibraryLoaderTask
{
    public void Execute(ILibraryViewModel viewModel)
    {
        if (viewModel.LoadStatus != LoadState.NotLoaded)
        {
            return;
        }
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(ILibraryViewModel viewModel)
    {
        viewModel.LoadStatus = LoadState.Loading;
        // load albums
        var albums =  _libraryRepo.GetPlaylists(PlaylistType.Album);
        if (albums.Count == 0)
        {
            albums =  _spotifyPlaylistRepo.GetPlaylists(PlaylistType.Album);
             _libraryRepo.AddPlaylists(albums);
        }
        AddPlaylistsToModel(viewModel, albums, true);

        //load playlists
        var playlists =  _libraryRepo.GetPlaylists(PlaylistType.Playlist);
        if (playlists.Count == 0)
        {
            playlists =  _spotifyPlaylistRepo.GetPlaylists(PlaylistType.Playlist);
             _libraryRepo.AddPlaylists(playlists);
        }
        AddPlaylistsToModel(viewModel, playlists, false);

    }

    private void AddPlaylistsToModel(ILibraryViewModel viewModel, List<Playlist> items, bool clearList)
    {
         _dispatcher.Dispatch(() =>
         {
             if (clearList)
             {
                 viewModel.Items.Clear();
             }

             foreach (var item in items)
             {
                 var playlistViewModel = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
                 playlistViewModel.Model = item;
                 playlistViewModel.LastAccessed = item.LastAccessed;
                 playlistViewModel.IsExpanded = false;
                 playlistViewModel.InLibrary = true;
                 viewModel.Items.Add(playlistViewModel);
             }

             if (!clearList)
             {
                 viewModel.LoadStatus = LoadState.Loaded;
             };
         });
    }
}
