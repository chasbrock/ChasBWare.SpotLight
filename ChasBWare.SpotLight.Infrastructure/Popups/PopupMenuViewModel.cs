using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public class PopupMenuViewModel(IPopupService _popupService) 
           : Notifyable,
             IPopupMenuViewModel
{
    private Size _size = new Size(200,100);
    public const string DefaultGroup = "";

    public ObservableCollection<IMenuItemGroup> MenuGroups { get; } = [];

    public async void Close() 
    {
        await _popupService.ClosePopupAsync();
    }

    public Size Size
    {
        get => _size;
        set => SetField(ref _size, value);
    }

    public void RecalcSize()
    {
        var totalHeight = 18 + (MenuGroups.Count - 1) * 4;
        foreach (var group in MenuGroups)
        {
            totalHeight += group.MenuItems.Where(mi => mi.Visible)
                                          .Sum(mi => (1 + mi.Caption.Length / 28) * 28);
        }
        Size = new Size(Size.Width, totalHeight);
    }

    public IMenuItem? FindMenuItem(PopupGroup group, PopupActivity activity)
    {
        var menuGroup = MenuGroups.FirstOrDefault(mg => mg.Group == group);
        if (menuGroup != null)
        {
            return menuGroup.MenuItems.FirstOrDefault(m => m.Activity == activity);
        }
        return null;
    }

    public IMenuItem AddItem(PopupActivity activity, string caption, Action<object?> action, string? toolTip = null, object? tag = null)
    {
        return AddItem(PopupGroup.Default, activity, caption, action, toolTip, tag);
    }

    public IMenuItem AddItem(PopupGroup popupGroup, PopupActivity activity, string caption, Action<object?> action, string? toolTip=null, object? tag=null) 
    {
        var group = MenuGroups.FirstOrDefault(g => g.Group == popupGroup);
        if (group == null)
        {
            group = new MenuItemGroup(popupGroup, this);
            MenuGroups.Add(group);
        }

        var newItem = new MenuItem(activity, action, caption, toolTip, tag);
       
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
