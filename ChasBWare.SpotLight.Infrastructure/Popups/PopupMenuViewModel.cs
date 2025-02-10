using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public class PopupMenuViewModel(IPopupService _popupService) 
           : Notifyable,
             IPopupMenuViewModel

{
    private int _height = 100;

    public const string DefaultGroup = "";

    public ObservableCollection<IMenuItemGroup> MenuGroups { get; } = [];

    public async void Close() 
    {
        await _popupService.ClosePopupAsync();
    }

    public int Height
    {
        get => _height;
        set => SetField(ref _height, value);
    }

    public int GetHeight()
    {
        var height = 4 + (MenuGroups.Count - 1) * 2;
        foreach (var group in MenuGroups)
        {
            height += group.MenuItems.Count(mi => mi.Visible) * 30;
        }
        return height;
    }

    public IMenuItem? FindMenuItem(object key)
    {
        foreach (var group in MenuGroups)
        {
            var found = group.MenuItems.FirstOrDefault(m => m.Key == key);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

    public IMenuItem AddItem(object key, string caption, Action<object?> action, string? toolTip = null, object? tag = null)
    {
        return AddItem(DefaultGroup, key, caption, action, toolTip, tag);
    }

    public IMenuItem AddItem(object groupKey, object key, string caption, Action<object?> action, string? toolTip=null, object? tag=null) 
    {
        var group = MenuGroups.FirstOrDefault(g => g.Key == groupKey);
        if (group == null)
        {
            group = new MenuItemGroup(groupKey, this);
            MenuGroups.Add(group);
        }

        var newItem = new MenuItem(key, action, caption, toolTip, tag);
       
        group.MenuItems.Add(newItem);
        return newItem;
    }

    public bool ShowSeparator(IMenuItemGroup menuItemGroup)
    {
        // if this is last group then never show separator
        if (MenuGroups.Count > 0 && MenuGroups[MenuGroups.Count - 1] == menuItemGroup) 
        {
            return false;
        }

        // never show if all our items are hidden
        if (!menuItemGroup.MenuItems.Any(m => m.Visible)) 
        {
            return false;
        }

        // find any groups that are below us that have active items
        bool found = false;
        foreach (var group in MenuGroups)
        {
            // we do not care about groups above us
            if (!found )
            {
                found = group == menuItemGroup;
                continue;
            }

            // we have a group below, has it any visible items?
            if (group.MenuItems.Any(m => m.Visible))
            {
                return true;
            }
        }
        return false;
    }
}
