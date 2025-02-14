using System.Globalization;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasbWare.Spotlight.UI.Converters;

public class DeviceTypesToStringConverter : IValueConverter
{
    public string? Computer { get; set; }
    public string? AVR { get; set; }
    public string? Tablet { get; set; }
    public string? Smartphone { get; set; }
    public string? Sonos { get; set; }
    public string? Unknown { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return string.Empty;
        }

        switch ((DeviceTypes)value)
        {
            case DeviceTypes.Computer:
                return Computer;
            case DeviceTypes.AVR:
                return AVR;
            case DeviceTypes.Tablet:
                return Tablet;
            case DeviceTypes.Smartphone:
                return Smartphone;
            case DeviceTypes.Sonos:
                return Sonos;
            default:
                return Unknown;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return DeviceTypes.Unknown;
        }

        if ((string)value == Computer)
        {
            return DeviceTypes.Computer;
        }
        if ((string)value == AVR)
        {
            return DeviceTypes.AVR;
        }
        if ((string)value == Tablet)
        {
            return DeviceTypes.Tablet;
        }
        if ((string)value == Smartphone)
        {
            return DeviceTypes.Smartphone;
        }
        if ((string)value == Sonos)
        {
            return DeviceTypes.Sonos;
        }
      
        return DeviceTypes.Unknown;
    }
}
