using ChasBWare.SpotLight.Infrastructure.Popups;
using CommunityToolkit.Maui.Views;
namespace ChasBWare.SpotLight.Popups;

public partial class LibraryPopupMenu : Popup
{
	public LibraryPopupMenu(LibraryPopupViewModel viewModel)
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