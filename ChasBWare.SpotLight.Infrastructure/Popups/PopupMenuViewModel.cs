using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public class PopupMenuViewModel(IPopupService _popupService) : Notifyable 
    {
        public const string DefaultGroup = "";

        public ObservableCollection<MenuItemGroup> MenuGroups { get; } = [];

        public async void Close() 
        {
            await _popupService.ClosePopupAsync();
        }    

        public MenuItem AddItem(string name, ICommand command, string? caption = null, string? toolTip = null, object? tag = null)
        {
            return AddItem(DefaultGroup, name, command, caption, toolTip, tag);
        }

        public MenuItem AddItem(string groupName, string name, ICommand command, string? caption = null, string? toolTip=null, object? tag=null) 
        {
            var group = MenuGroups.FirstOrDefault(g => g.Name == groupName);
            if (group == null)
            {
                group = new MenuItemGroup(groupName, this);
                MenuGroups.Add(group);
            }

            var newItem = new MenuItem(name, command, caption, toolTip, tag);
           
            group.MenuItems.Add(newItem);
            return newItem;
        }

        internal bool ShowSeparator(MenuItemGroup menuItemGroup)
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
                if (!found && group != menuItemGroup)
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
}
