using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Converters
{
    internal class TrackStatusToStringConverter : IValueConverter
    {
        public string? PausedValue { get; set; }
        public string? PlayingValue { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            switch ((TrackStatus)value)
            {
                case TrackStatus.Paused:
                    return PausedValue ?? string.Empty;
                case TrackStatus.Playing:
                    return PlayingValue ?? string.Empty;
                default:
                    return string.Empty;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return TrackStatus.NotPlaying; 
            }

            if ((string)value == PausedValue)
            {
                return TrackStatus.Paused;
            }
            if ((string)value == PlayingValue)
            {
                return TrackStatus.Playing;
            }
            return TrackStatus.NotPlaying;
        }
    }
}
