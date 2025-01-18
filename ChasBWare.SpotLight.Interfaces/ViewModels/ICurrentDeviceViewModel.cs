using ChasBWare.SpotLight.Domain.Models;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    /// <summary>
    /// controls the currently selected device.
    /// </summary>
    public interface ICurrentDeviceViewModel
    {
        /// <summary>
        /// the currently selected device
        /// </summary>
        DeviceModel? SelectedDevice { get; set; }

        /// <summary>
        /// true if there is an active device
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// turns muting on or off
        /// </summary>
        ICommand MuteCommand { get; }

        /// <summary>
        /// muting sets volume to zero, but remebers the old value
        /// </summary>
        bool IsMuted { get; set; }

        /// <summary>
        /// volume as percentage. Setting volume will turn off muting
        /// </summary>
        int Volume { get; set; }

        /// <summary>
        /// display image on button
        /// </summary>
        string IsMutedCommandImage { get; }
    }
}
