using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Models;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public partial class DeviceViewModel: Notifyable, IDeviceViewModel
    {
        public DeviceModel Model { get; set; } = new DeviceModel { Id = "", Name = "", DeviceType = "" };

        public string Name { get => Model.Name;  } 
        public string DeviceType { get => Model.DeviceType; }

        public string DeviceImage
        {
            get
            {
                switch (DeviceType.ToUpper())
                {
                    case "COMPUTER":
                        return "icons8_computer.png";
                    case "AVR":
                        return "icons8_av_receiver.png";
                    case "TABLET":
                        return "icons8_tablet.png";
                    case "SMARTPHONE":
                        return "icons8_phone.png";
                    case "SONOS":
                        return "icons8_sonos.png";
                    default:
                        return "icons8_sound_other.png";
                }
            }
        }

        public bool IsActive
        {
            get => Model.IsActive;
            set => SetField(Model, value);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
