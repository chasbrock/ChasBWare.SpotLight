using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;

namespace ChasBWare.SpotLight.Infrastructure.Services
{
    public class HatedService(IServiceProvider _serviceProvider,
                              IUserRepository _userRepository) 
               : IHatedService
    {
        private HashSet<string> _hatedItems = [];

        public bool Initialised { get; private set; } = false;

        public bool IsHated(string? itemId)
        {
            if (itemId == null) {
                return false;
            }
            return _hatedItems.Contains(itemId);
        }

        public async void Refresh()
        {
            Initialised = true;
            var hatedRepo = _serviceProvider.GetService<IHatedItemsRepository>();
            if (hatedRepo != null)
            {
                _hatedItems = await hatedRepo.GetItems(_userRepository.CurrentUserId);
            }
        }
    }
}
