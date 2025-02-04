using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class LoadRecentPlaylistTask(IServiceProvider _serviceProvider,
                                        IDispatcher _dispatcher,
                                        IPlaylistRepository _playlistRepository,
                                        IUserRepository _userRepository)
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
                        var playlistViewModel = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
                        playlistViewModel.Model = item;
                        playlistViewModel.LastAccessed = item.LastAccessed;
                        playlistViewModel.IsSaved = false;
                        viewModel.Items.Add(playlistViewModel);
                    }

                    viewModel.LoadStatus = LoadState.Loaded;
                });
            }
        }
    }

}
