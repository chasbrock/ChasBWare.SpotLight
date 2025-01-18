using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class QueueViewModel(IServiceProvider _serviceProvider) : TrackListViewModel, IQueueViewModel
    {
        public void LoadQueue()
        {
            var task = _serviceProvider.GetService<IQueueLoaderTask>();
            task?.Execute(this);
        }
    }
}
