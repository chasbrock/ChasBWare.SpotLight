using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IDeviceListViewModel
    {
        ObservableCollection<IDeviceViewModel> Devices { get; }
        IDeviceViewModel? SelectedDevice { get; set; }
        ICommand OpenPopupCommand { get; }
    }
}
