using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Pages;

public partial class ArtistPage : ContentPage
{
	public ArtistPage(IRecentArtistsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}