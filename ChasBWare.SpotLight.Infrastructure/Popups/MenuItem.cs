using ChasBWare.SpotLight.Infrastructure.Utility;
using Microsoft.Maui;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public class MenuItem : Notifyable
    {
        private readonly ICommand _command;
        private string _caption;
        private bool _enabled = true;
        private bool _visible = true;
        private string? _toolTip;

        public MenuItem(string name, ICommand command, string? caption = null, string? toolTip = null, object? tag = null) 
        {
            Name = name;
            _caption = caption ?? Name;
            _command = command;
            _toolTip = toolTip;
            Tag = tag;
            Click = new Command(p => _command?.Execute(Tag));
        }

        public string Name { get; }
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
    }
}
