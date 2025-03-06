using ChasBWare.SpotLight.Domain.Models;

namespace ChasBWare.SpotLight.Domain.Entities;

public class CurrentContext
{
    public required DeviceModel Device { get; set; }
    public required PlayingTrack Track { get; set; }
}
