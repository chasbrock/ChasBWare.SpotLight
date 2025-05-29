using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Models;

namespace ChasBWare.SpotLight.Mappings.Mappers;

public static class DeviceMapper
{
    public static DeviceModel CopyToDevice(this SpotifyAPI.Web.Device source)
    {
        return new DeviceModel
        {
            Id = source.Id,
            Name = source.Name,
            DeviceType = source.Type.ToDeviceType(),
            RawDeviceType = source.Type,
            IsActive = source.IsActive,
            IsMuted = source.VolumePercent == null,
            SupportsVolume = source.SupportsVolume,
            VolumePercent = source.VolumePercent ?? 0
        };
    }
}
