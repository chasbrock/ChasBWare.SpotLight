using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class LibraryPopupViewModel
                   : PopupMenuViewModel
{
    public LibraryPopupViewModel(IPopupService popupService,
                                           IPopupItemService _popupItemService,
                                           ILibraryViewModel _library)
         : base(popupService)
    {
        MenuGroups.Clear();
        _popupItemService.AddMenuItem(this, _library, PopupActivity.ExpandAll);
        _popupItemService.AddMenuItem(this, _library, PopupActivity.CollapseAll);

        RecalcSize();
    }
}

