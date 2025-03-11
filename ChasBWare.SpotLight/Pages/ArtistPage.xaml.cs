using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Pages;

public partial class ArtistPage : ContentPage
{
	public ArtistPage(IRecentArtistsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        ((IRecentArtistsViewModel)BindingContext).PlayerControlViewModel.NotifyAll();
    }
}