using System.Collections.ObjectModel;
using ChasBWare.SpotLight.Definitions.Enums;
using Microsoft.Maui.Graphics;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public interface IPopupMenuViewModel
{
    ObservableCollection<IMenuItemGroup> MenuGroups { get; }

    public void Close();
    public Size Size { get; set; }
    bool ShowSeparator(IMenuItemGroup group);
    public void RecalcSize();
    public IMenuItem? FindMenuItem(PopupGroup group, PopupActivity activity);
    public IMenuItem AddItem(PopupActivity activity, string caption, Action<object?> action, string? toolTip = null, object? tag = null);
    public IMenuItem AddItem(PopupGroup group, PopupActivity activity, string caption, Action<object?> action, string? toolTip = null, object? tag = null);
   
}
