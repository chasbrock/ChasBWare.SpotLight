using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public class MenuItem : Notifyable, IMenuItem
    {
        private readonly Action<object?> _action;
        private string _caption;
        private bool _enabled = true;
        private bool _visible = true;
        private string? _toolTip;

        public MenuItem(PopupActivity activity, Action<object?> action, string caption, string? toolTip = null, object? tag = null)
        {
            Activity = activity;
            _caption = caption;
            _action = action;
            _toolTip = toolTip;
            Tag = tag;
            Click = new Command(p => _action?.Invoke(Tag));
        }

        public PopupActivity Activity { get; }
        public ICommand Click { get; }
        public object? Tag { get; set; }

        public string Caption
        {
            get => _caption;
            set => SetField(ref _caption, value);
        }

        public string? ToolTip
        {
            get => _toolTip;
            set => SetField(ref _toolTip, value);
        }

        public bool Enabled
        {
            get => _enabled;
            set => SetField(ref _enabled, value);
        }

        public bool Visible
        {
            get => _visible;
            set => SetField(ref _visible, value);
        }

        public override string ToString()
        {
            return Caption;
        }
    }
}
