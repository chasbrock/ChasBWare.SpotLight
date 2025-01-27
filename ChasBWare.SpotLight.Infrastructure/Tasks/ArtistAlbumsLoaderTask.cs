using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class ArtistAlbumsLoaderTask(IArtistRepository _artistRepository,
                                        IUserRepository _userRepository, 
                                        ISpotifyArtistRepository _spotifyArtistRepository,
                                        IServiceProvider _serviceProvider,
                                        IDispatcher _dispatcher) 
               : IArtistAlbumsLoaderTask
    {
        public async void Execute(IArtistViewModel viewModel)
        {
            // try to get from database
            var albums = await _artistRepository.LoadArtistAlbums(viewModel.Id);
            if (albums.Count == 0 )
            {
                albums = await _spotifyArtistRepository.LoadArtistAlbums(viewModel.Id);
                await _artistRepository.AddRecentArtistAndAlbums(_userRepository.CurrentUserId, viewModel.Model, albums);
            }

            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();
                foreach (var album in albums)
                {
                    var albumViewModel = _serviceProvider.GetService<IPlaylistViewModel>();
                    if (albumViewModel != null)
                    {
                        albumViewModel.Model = album;
                        albumViewModel.IsTracksExpanded = false;
                        viewModel.Items.Add(albumViewModel);
                    }
                }

                viewModel.LoadStatus = LoadState.Loaded;
                viewModel.UpdateSorting();
            });
        }
    }
}
