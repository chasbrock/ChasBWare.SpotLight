using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Pages;

public partial class DevicesPage : ContentPage
{
    public DevicesPage(IDeviceListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}