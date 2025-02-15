using ChasBWare.SpotLight.Infrastructure.Popups;
using CommunityToolkit.Maui.Views;

namespace ChasBWare.SpotLight.Popups;

public partial class TrackPopupMenu : Popup
{
	public TrackPopupMenu(TrackPopupViewModel viewModel)
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