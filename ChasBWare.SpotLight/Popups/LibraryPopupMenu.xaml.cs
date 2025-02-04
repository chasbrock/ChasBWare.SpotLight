using ChasBWare.SpotLight.Infrastructure.Popups;
using CommunityToolkit.Maui.Views;

namespace ChasBWare.SpotLight.Popups;

public partial class LibraryPopupMenu : Popup
{
	public LibraryPopupMenu(LibraryPopupViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}