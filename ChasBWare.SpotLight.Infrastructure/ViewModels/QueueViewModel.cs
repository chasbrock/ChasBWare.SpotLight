using ChasBWare.SpotLight.Definitions.Tasks.Queue;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class QueueViewModel(IServiceProvider serviceProvider,
                              INavigator navigator,
                              IPopupService popupService)
               : TrackListViewModel(serviceProvider, navigator, popupService),
                 IQueueViewModel
    {
        public void LoadQueue()
        {
            var task = _serviceProvider.GetService<IQueueLoaderTask>();
            task?.Execute(this);
        }
    }
}
