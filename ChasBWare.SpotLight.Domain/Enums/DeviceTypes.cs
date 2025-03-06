namespace ChasBWare.SpotLight.Domain.Enums;

public enum DeviceTypes
{
    Unknown,
    Computer,
    Tablet,
    Smartphone,
    AVR,
    Sonos
}

public static class DeviceTypeUtils
{
    public static DeviceTypes ToDeviceType(this string deviceType)
    {
        switch (deviceType.ToUpper())
        {
            case "COMPUTER":
                return DeviceTypes.Computer;
            case "AVR":
                return DeviceTypes.AVR;
            case "TABLET":
                return DeviceTypes.Tablet;
            case "SMARTPHONE":
                return DeviceTypes.Smartphone;
            case "SONOS":
                return DeviceTypes.Sonos;
            default:
                return DeviceTypes.Unknown;
        }
    }
}