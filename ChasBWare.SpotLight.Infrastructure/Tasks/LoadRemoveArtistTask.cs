using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class LoadRemoveArtistTask(IArtistRepository _artistRepository,
                                      IUserRepository _userRepository) 
               : ILoadRemoveArtistTask
    {
        public async void Execute(IRecentArtistsViewModel viewModel, string artistId)
        {
           await _artistRepository.Remove(_userRepository.CurrentUser.Id, artistId);
        }
    }

}
