using ChasBWare.SpotLight.Infrastructure.Utility;
using System.Collections.ObjectModel;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public class MenuItemGroup : Notifyable
    {
        private bool _visible = true;
        
        public MenuItemGroup(string name, PopupMenuViewModel owner)
        {
            Name = name;
            Owner = owner;
        }

        public ObservableCollection<MenuItem> MenuItems { get; } = [];
        
        public string Name { get;}
        public PopupMenuViewModel Owner { get; }
    
        public bool ShowSeparator
        {
            get => Owner.ShowSeparator(this);
        }

        public bool Visible
        {
            get => _visible;
            set => SetField(ref _visible, value);
        }
    }
}
