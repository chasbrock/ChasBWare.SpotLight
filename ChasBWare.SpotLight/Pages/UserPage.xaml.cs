using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Pages;

public partial class UserPage : ContentPage
{
	public UserPage(IRecentUserViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        ((IRecentUserViewModel)BindingContext).PlayerControlViewModel.NotifyAll();
    }
}