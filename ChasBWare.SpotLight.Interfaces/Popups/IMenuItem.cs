using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public interface IMenuItem
    {
        string Caption { get; set; }
        ICommand Click { get; }
        bool Enabled { get; set; }
        PopupActivity Activity { get; }
        object? Tag { get; set; }
        string? ToolTip { get; set; }
        bool Visible { get; set; }
    }
}
