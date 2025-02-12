using System.Collections.ObjectModel;
using ChasBWare.SpotLight.Definitions.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public interface IMenuItemGroup
    {
        ObservableCollection<IMenuItem> MenuItems { get; }
        PopupGroup Group { get; }
        IPopupMenuViewModel Owner { get; }
        bool ShowSeparator { get; }
        bool Visible { get; set; }
    }
}
