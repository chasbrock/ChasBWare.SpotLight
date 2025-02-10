using ChasBWare.SpotLight.Infrastructure.Popups;
using CommunityToolkit.Maui.Views;

namespace ChasBWare.SpotLight.Popups;

public partial class RecentArtistPopupMenu : Popup
{
	public RecentArtistPopupMenu(RecentArtistPopupViewModel viewModel)
	{
		CanBeDismissedByTappingOutsideOfPopup = true;
		BindingContext = viewModel;

        Content = new PopupMenu
        {
            BindingContext = viewModel,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        }; 
    }
}


