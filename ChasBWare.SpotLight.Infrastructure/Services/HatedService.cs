using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;

namespace ChasBWare.SpotLight.Infrastructure.Services
{
    public class HatedService(IServiceProvider _serviceProvider) 
               : IHatedService
    {
        private HashSet<string> _hatedItems = [];

        public bool Initialised { get; private set; } = false;

        public bool GetIsHated(string? itemId)
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
                _hatedItems = await hatedRepo.GetItems();
            }
        }

        public void SetIsHated(string itemId, bool isHated)
        {
            if (isHated)
            {
                _hatedItems.Add(itemId);
            }
            else
            {
                _hatedItems.Remove(itemId);
            }

            Task.Run(() =>
            {
                var hatedRepo = _serviceProvider.GetService<IHatedItemsRepository>();
                if (hatedRepo != null)
                {
                    hatedRepo.SetHated(itemId, isHated);
                }
            });

        }
    }
}
