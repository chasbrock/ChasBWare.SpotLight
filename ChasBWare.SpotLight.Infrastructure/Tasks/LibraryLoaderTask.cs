using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
 
    public class LibraryLoaderTask(IUserRepository _userRepository,
                                   IPlaylistRepository _playlistRepository,
                                   ISpotifyPlaylistRepository _spotifyPlaylistRepository,
                                   IServiceProvider _serviceProvider,
                                   IDispatcher _dispatcher)
               : ILibraryLoaderTask
    {
        public async void Execute(ILibraryViewModel viewModel)
        {
            if (_userRepository.CurrentUser?.Id == null ||
                viewModel.LoadStatus != LoadState.NotLoaded)
            {
                return;
            }

            viewModel.LoadStatus = LoadState.Loading;
            // load albums
            var albums = await _playlistRepository.GetPlaylists(_userRepository.CurrentUser.Id, PlaylistType.Album, true);
            if (albums.Count == 0)
            {
                albums = await _spotifyPlaylistRepository.GetPlaylists(PlaylistType.Album);
            }
            AddPlaylistsToModel(viewModel, albums, true);

            //load playlists
            var playlists = await _playlistRepository.GetPlaylists(_userRepository.CurrentUser.Id, PlaylistType.Playlist, true);
            if (playlists.Count == 0)
            {
                playlists = await _spotifyPlaylistRepository.GetPlaylists(PlaylistType.Playlist);
            }
            AddPlaylistsToModel(viewModel, playlists, false);
            viewModel.LoadStatus = LoadState.Loaded;

        }

        private void AddPlaylistsToModel(ILibraryViewModel viewModel, List<Tuple<Playlist, DateTime>> items, bool clearList)
        {
             _dispatcher.Dispatch(() =>
             {
                 if (clearList)
                 {
                     viewModel.Items.Clear();
                 }

                foreach (var item in items)
                {
                     var playlistViewModel = _serviceProvider.GetService<IPlaylistViewModel>();
                     if (playlistViewModel != null)
                     {
                         playlistViewModel.Model = item.Item1;
                         playlistViewModel.LastAccessed = item.Item2;
                         playlistViewModel.IsTracksExpanded = true;
                         viewModel.Items.Add(playlistViewModel);
                     } 
                }
            });
        }
    }
}
