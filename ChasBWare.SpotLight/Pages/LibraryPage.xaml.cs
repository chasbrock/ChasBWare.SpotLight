using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Pages;

public partial class LibraryPage : ContentPage
{
	public LibraryPage(ILibraryViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        ((ILibraryViewModel)BindingContext).PlayerControlViewModel.NotifyAll();
    }
}
