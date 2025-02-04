using ChasBWare.SpotLight.Infrastructure.Utility;
using System.Collections.ObjectModel;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public class MenuItemGroup(object key, IPopupMenuViewModel owner) : Notifyable, IMenuItemGroup
    {
        private bool _visible = true;
              
  
        public object Key { get; } = key;
        public IPopupMenuViewModel Owner { get; } = owner;
        public ObservableCollection<IMenuItem> MenuItems { get; } = [];

        public bool ShowSeparator
        {
            get => Owner.ShowSeparator(this);
        }

        public bool Visible
        {
            get => _visible;
            set => SetField(ref _visible, value);
        }

        public override string ToString()
        {
            return $"{Key}";
        }
    }
}
