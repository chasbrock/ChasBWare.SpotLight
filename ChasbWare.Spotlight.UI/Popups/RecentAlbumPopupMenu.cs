using ChasBWare.SpotLight.Infrastructure.Popups;
using CommunityToolkit.Maui.Views;

namespace ChasBWare.SpotLight.UI.Popups;

public partial class RecentAlbumPopupMenu : Popup
{
    public RecentAlbumPopupMenu(RecentAlbumPopupViewModel viewModel)
    {
        CanBeDismissedByTappingOutsideOfPopup = true;
        BindingContext = viewModel;
        Content = new PopupMenu
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BindingContext = viewModel,
        };
        this.SetBinding(Popup.SizeProperty, nameof(IPopupMenuViewModel.Size));
    }
}