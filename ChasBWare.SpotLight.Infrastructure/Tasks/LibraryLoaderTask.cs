using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
 
    public class LibraryLoaderTask(IServiceProvider _serviceProvider,
                                   IDispatcher _dispatcher,
                                   IUserRepository _userRepository,
                                   IPlaylistRepository _playlistRepository,
                                   ISpotifyPlaylistRepository _spotifyPlaylistRepository                                   )
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
            var albums =  _playlistRepository.GetPlaylists(_userRepository.CurrentUserId, PlaylistType.Album, true);
            if (albums.Count == 0)
            {
                albums =  _spotifyPlaylistRepository.GetPlaylists(PlaylistType.Album);
                 _playlistRepository.AddPlaylists(albums, _userRepository.CurrentUserId, true);
            }
            AddPlaylistsToModel(viewModel, albums, true);

            //load playlists
            var playlists =  _playlistRepository.GetPlaylists(_userRepository.CurrentUserId, PlaylistType.Playlist, true);
            if (playlists.Count == 0)
            {
                playlists =  _spotifyPlaylistRepository.GetPlaylists(PlaylistType.Playlist);
                 _playlistRepository.AddPlaylists(playlists, _userRepository.CurrentUserId, true);
            }
            AddPlaylistsToModel(viewModel, playlists, false);
  
        }

        private void AddPlaylistsToModel(ILibraryViewModel viewModel, List<RecentPlaylist> items, bool clearList)
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
                     playlistViewModel.IsSaved = true;
                     viewModel.Items.Add(playlistViewModel);
                 }

                 if (!clearList)
                 {
                     viewModel.LoadStatus = LoadState.Loaded;
                 };
             });
        }
    }
}
