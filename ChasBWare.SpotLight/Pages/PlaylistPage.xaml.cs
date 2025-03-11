using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Pages;

public partial class PlaylistPage : ContentPage
{
	public PlaylistPage(IRecentPlaylistsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        ((IRecentPlaylistsViewModel)BindingContext).PlayerControlViewModel.NotifyAll();
    }
}