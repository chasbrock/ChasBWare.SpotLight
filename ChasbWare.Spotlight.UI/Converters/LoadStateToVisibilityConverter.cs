using System.Globalization;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasbWare.Spotlight.UI.Converters;

public class LoadStateToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        { 
            return null; 
        }
        
        switch ((LoadState)value)
        {
            case LoadState.Loaded:
                return true;
            default:
                return false;

        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        switch ((Visibility)value)
        {
            case Visibility.Visible:
                return LoadState.Loaded;
            default:
                return LoadState.NotLoaded;

        }
    }
}
