using System.Collections.ObjectModel;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public interface IMenuItemGroup
    {
        ObservableCollection<IMenuItem> MenuItems { get; }
        object Key { get; }
        IPopupMenuViewModel Owner { get; }
        bool ShowSeparator { get; }
        bool Visible { get; set; }
    }
}
