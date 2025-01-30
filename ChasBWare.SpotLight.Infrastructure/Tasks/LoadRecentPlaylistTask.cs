using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class LoadRecentPlaylistTask(IPlaylistRepository _playlistRepository,
                                     IUserRepository _userRepository,
                                     IServiceProvider _serviceProvider,
                                     IDispatcher _dispatcher)
               : ILoadRecentPlaylistTask
    {
        public  void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, PlaylistType playlistType)
        {
            Task.Run(() => RunTask(viewModel, playlistType));
        }

        private async void RunTask(IRecentViewModel<IPlaylistViewModel> viewModel, PlaylistType playlistType)
        {
            var items = await _playlistRepository.GetPlaylists(_userRepository.CurrentUserId, playlistType, true);
            if (items.Count == 0)
            {
                _dispatcher.Dispatch(() =>
                {
                    viewModel.Items.Clear();

                    foreach (var item in items)
                    {
                        var playlistViewModel = _serviceProvider.GetService<IPlaylistViewModel>();
                        if (playlistViewModel != null)
                        {
                            playlistViewModel.Model = item;
                            playlistViewModel.LastAccessed = item.LastAccessed;
                            viewModel.Items.Add(playlistViewModel);
                        }
                    }
                    viewModel.LoadStatus = LoadState.Loaded;
                });
            }
        }
    }

}
