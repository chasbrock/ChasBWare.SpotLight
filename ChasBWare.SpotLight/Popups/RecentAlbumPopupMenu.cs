using ChasBWare.SpotLight.Infrastructure.Popups;
using CommunityToolkit.Maui.Views;

namespace ChasBWare.SpotLight.Popups;

public partial class RecentAlbumPopupMenu : Popup
{
	public RecentAlbumPopupMenu(RecentAlbumPopupViewModel viewModel)
	{
        CanBeDismissedByTappingOutsideOfPopup = true;
        BindingContext = viewModel;
        Content = new PopupMenu
        {
            WidthRequest = 200,
            HeightRequest = 100,

            BindingContext = viewModel,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

    }
}