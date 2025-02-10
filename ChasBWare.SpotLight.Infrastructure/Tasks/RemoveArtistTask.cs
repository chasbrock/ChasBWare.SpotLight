using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class RemoveArtistTask(IDispatcher _dispatcher,
                                  IArtistRepository _artistRepository,
                                  IUserRepository _userRepository) 
               : IRemoveRecentArtistTask
    {
    
        public void Execute(IRecentArtistsViewModel viewModel, IArtistViewModel item)
        {
            Task.Run(() => RunTask(viewModel, item));
        }

        public void Execute(IRecentArtistsViewModel viewModel)
        {
            Task.Run(() => RunTask(viewModel));
        }

        private async void RunTask(IRecentArtistsViewModel viewModel)
        {
            if (await _artistRepository.RemoveAll(_userRepository.CurrentUserId))
            {
                _dispatcher.Dispatch(() =>
                {
                    viewModel.Items.Clear();
                    viewModel.SelectedItem = null;
                    viewModel.RefreshView();
                });
            }
        }

        private async void RunTask(IRecentArtistsViewModel viewModel, IArtistViewModel item)
        {
            if (await _artistRepository.Remove(_userRepository.CurrentUserId, item.Id))
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
