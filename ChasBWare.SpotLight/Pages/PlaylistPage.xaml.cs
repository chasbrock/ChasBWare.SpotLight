using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Pages;

public partial class PlaylistPage : ContentPage
{
	public PlaylistPage(IRecentPlaylistsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}