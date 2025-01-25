using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class UpdateLastAccessedTask(IRecentItemRepository _recentItemRepository,
                                        IUserRepository _userRepository)
               : IUpdateLastAccessedTask
    {
        public async void Execute(string itemId, DateTime lastAccessed, bool isSaved)
        {
            await _recentItemRepository.UpdateLastAccessed(_userRepository.CurrentUserId, itemId, lastAccessed, isSaved);
        }
    }

}
