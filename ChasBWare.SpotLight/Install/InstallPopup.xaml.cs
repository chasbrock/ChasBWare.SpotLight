using CommunityToolkit.Maui.Views;

namespace ChasBWare.SpotLight.Install;

public partial class InstallPopup : Popup
{
    public InstallPopup(InstallViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
