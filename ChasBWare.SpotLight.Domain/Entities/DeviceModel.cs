using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Domain.Models
{
    public class DeviceModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required DeviceTypes DeviceType { get; set; }
        public bool IsActive { get; set; }
        public bool SupportsVolume { get; set; }
        public int VolumePercent { get; set; }
        public bool IsMuted { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
