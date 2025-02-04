using ChasBWare.SpotLight.Infrastructure.Popups;
using CommunityToolkit.Maui.Views;

namespace ChasBWare.SpotLight.Popups;

public partial class TracksPopupMenu : Popup
{
	public TracksPopupMenu(TrackPopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}