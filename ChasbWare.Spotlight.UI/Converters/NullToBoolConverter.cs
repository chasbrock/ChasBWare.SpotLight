using System.Globalization;
using ChasBWare.SpotLight.Definitions.Enums;

namespace ChasbWare.Spotlight.UI.Converters;

public class NullToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
