using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Models;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public partial class DeviceViewModel: Notifyable, IDeviceViewModel
    {
        private bool _isMuted = false;
        private string _mutedCommandImage = SetMutedCommandImage(false);
     
        public DeviceViewModel()
        {
            MuteCommand = new Command(DoMuteCommand);
        }

        public DeviceModel Model { get; set; } = new DeviceModel { Id = "", Name = "", DeviceType = "" };

        public string Name { get => Model.Name;  } 
        public string DeviceType { get => Model.DeviceType; }

        public string DeviceImage
        {
            get
            {
                switch (DeviceType)
                {
                    case "Computer":
                        return "icons8_computer.png";
                    case "AVR":
                        return "icons8_av_receiver.png";
                    case "Tablet":
                        return "icons8_tablet.png";
                    case "Smartphone":
                        return "icons8_phone.png";
                    case "Sonos":
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

        public int VolumePercent
        {
            get => Model.VolumePercent;
            set => SetField(Model, value);
        }

        public string IsMutedCommandImage
        {
            get => _mutedCommandImage;
            set => SetField(ref _mutedCommandImage, value);
        }

        public ICommand MuteCommand { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        private void DoMuteCommand()
        {
            _isMuted = !_isMuted;
            IsMutedCommandImage = SetMutedCommandImage(_isMuted);
        }

        private static string SetMutedCommandImage(bool isMuted)
        {
            return isMuted ? "icons8_no_audio.png" : "icons8_audio.png";
        }
    }
}
