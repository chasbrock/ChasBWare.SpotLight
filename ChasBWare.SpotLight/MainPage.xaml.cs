using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IMainWindowViewModel viewModel)
        {
            InitializeComponent();
            
            BindingContext = viewModel;
        }
    }

}
