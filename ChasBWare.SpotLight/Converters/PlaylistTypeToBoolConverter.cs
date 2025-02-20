using System.Globalization;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasbWare.Spotlight.Converters;

public class PlaylistTypeToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return false;
        }

        return ((PlaylistType)value)  != PlaylistType.Playlist;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
