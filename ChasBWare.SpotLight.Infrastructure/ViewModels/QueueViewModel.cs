using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class QueueViewModel(IPopupService popupService,
                                IServiceProvider _serviceProvider)
               : TrackListViewModel(popupService),
                 IQueueViewModel
    {
        public void LoadQueue()
        {
            var task = _serviceProvider.GetService<IQueueLoaderTask>();
            task?.Execute(this);
        }
    }
}
