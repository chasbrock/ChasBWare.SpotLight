using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class RemoveArtistTask(IArtistRepository _artistRepository,
                                      IUserRepository _userRepository) 
               : IRemoveArtistTask
    {
        public void Execute(IRecentArtistsViewModel viewModel, string artistId)
        {
            Task.Run(() => RunTask(viewModel, artistId));
        }

        private async void RunTask(IRecentArtistsViewModel viewModel, string artistId)
        {
            await _artistRepository.Remove(_userRepository.CurrentUserId, artistId);
        }
    }

}
