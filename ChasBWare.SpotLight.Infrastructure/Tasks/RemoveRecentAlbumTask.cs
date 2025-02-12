using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Repositories;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class RemoveRecentAlbumTask(IDispatcher _dispatcher,
                                  IPlaylistRepository _playlistRepository,
                                  IUserRepository _userRepository)
               : IRemoveRecentAlbumTask
    {
        public void Execute(IRecentAlbumsViewModel viewModel, IPlaylistViewModel item)
        {
            Task.Run(() => RunTask(viewModel, item));
        }

        public void Execute(IRecentAlbumsViewModel viewModel)
        {
            Task.Run(() => RunTask(viewModel));
        }

        private void RunTask(IRecentAlbumsViewModel viewModel)
        {
            if (_playlistRepository.RemoveUnsavedPlaylists(_userRepository.CurrentUserId, Domain.Enums.PlaylistType.Album))
            {
                _dispatcher.Dispatch(() =>
                {
                    viewModel.Items.Clear();
                    viewModel.SelectedItem = null;
                    viewModel.RefreshView();
                });
            }
        }

        private void RunTask(IRecentAlbumsViewModel viewModel, IPlaylistViewModel item)
        {
            if (_playlistRepository.RemoveUnsavedPlaylist(_userRepository.CurrentUserId, item.Id))
            {
                _dispatcher.Dispatch(() =>
                {
                    viewModel.Items.Remove(item);
                    viewModel.SelectedItem = null;
                    viewModel.RefreshView();
                });
            }
        }
    }
}
