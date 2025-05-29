using System.Collections.ObjectModel;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public class MenuItemGroup(PopupGroup group, IPopupMenuViewModel owner)
               : Notifyable,
                 IMenuItemGroup
    {
        private bool _visible = true;

        public PopupGroup Group { get; } = group;
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
            return $"{Group}";
        }
    }
}
