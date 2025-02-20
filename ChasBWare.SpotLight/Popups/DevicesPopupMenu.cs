using ChasBWare.SpotLight.Infrastructure.Popups;
using CommunityToolkit.Maui.Views;

namespace ChasBWare.SpotLight.Popups;

public partial class DevicesPopupMenu : Popup
{
	public DevicesPopupMenu(RecentArtistPopupViewModel viewModel)
	{
        CanBeDismissedByTappingOutsideOfPopup = true;
        BindingContext = viewModel;
        Content = new PopupMenu
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BindingContext = viewModel
        };
        this.SetBinding(Popup.SizeProperty, nameof(IPopupMenuViewModel.Size));
    }
}


