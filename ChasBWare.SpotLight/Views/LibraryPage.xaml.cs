using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Views;

public partial class LibraryPage : ContentPage
{
	public LibraryPage(ILibraryViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}