using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IQueueViewModel : ITrackListViewModel
    {
        void LoadQueue();
    }
}
