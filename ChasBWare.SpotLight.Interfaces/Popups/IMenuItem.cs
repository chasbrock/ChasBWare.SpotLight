using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public interface IMenuItem
    {
        string Caption { get; set; }
        ICommand Click { get; }
        bool Enabled { get; set; }
        object Key { get; }
        object? Tag { get; set; }
        string? ToolTip { get; set; }
        bool Visible { get; set; }
    }
}
