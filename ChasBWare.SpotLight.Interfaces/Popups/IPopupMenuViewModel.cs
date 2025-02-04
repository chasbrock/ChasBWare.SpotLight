using System.Collections.ObjectModel;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public interface IPopupMenuViewModel
{
    ObservableCollection<IMenuItemGroup> MenuGroups { get; }

    public void Close();
    public int Height { get; set; }
    bool ShowSeparator(IMenuItemGroup group);
    public int GetHeight();
    public IMenuItem? FindMenuItem(object key);
    public IMenuItem AddItem(object key, string caption, Action<object?> action, string? toolTip = null, object? tag = null);
    public IMenuItem AddItem(object groupKey, object key, string caption, Action<object?> action, string? toolTip = null, object? tag = null);
   
}
