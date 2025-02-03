using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{

    public class ArtistAlbumsLoaderTask(IServiceProvider _serviceProvider,
                                        IDispatcher _dispatcher,
                                        ISpotifyArtistRepository _spotifyArtistRepo,
                                        IArtistRepository _artistRepo,
                                        IUserRepository _userRepository)
               : IArtistAlbumsLoaderTask
    {
        public void Execute(IArtistViewModel viewModel)
        {
            Task.Run(() => RunTask(viewModel));
        }

        private async void RunTask(IArtistViewModel viewModel)
        {
            // try to get from database
            var albums = await _artistRepo.LoadArtistAlbums(viewModel.Id);
            if (albums.Count == 0)
            {
                albums = await _spotifyArtistRepo.LoadArtistAlbums(viewModel.Id);
                await _artistRepo.AddRecentArtistAndAlbums(_userRepository.CurrentUserId, viewModel.Model, albums);
            }

            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();
                foreach (var album in albums)
                {
                    var albumViewModel = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
                    albumViewModel.Model = album;
                    albumViewModel.IsExpanded = false;
                    viewModel.Items.Add(albumViewModel);
                }

                viewModel.LoadStatus = LoadState.Loaded;
                viewModel.UpdateSorting();
            });
        }
    }
}
