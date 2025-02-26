using System.Text;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Utility;

public static class Misc
{
    public static string MSecsToMinsSecs(this int source)
    {
        source /= 1000;
        int hours = source / 60 / 60;
        int mins = (source - hours * 60 * 60) / 60;
        int secs = source - hours * 60 * 60 - mins * 60;
        return hours > 0 ? $"{hours}:{mins}:{secs:00}" : $"{mins}:{secs:00}";
    }

    public static string StripQuotes(this string value)
    {
        value = value.Trim();
        if (string.IsNullOrWhiteSpace(value) || value.Length <= 2)
            return string.Empty;
        if (value[0] == '"' && value[value.Length - 1] == '"')
            return value.Substring(1, value.Length - 2).Trim();
        return value;
    }


    public static bool IsNumeric(this object value)
    {
        return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
    }

    public static string CamelCaseToWords(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return string.Empty;
        }
        source = source.Trim();
        var builder = new StringBuilder(source.Length * 2);

        var c = source[0];
        builder.Append(char.ToUpper(c));

        // more than 1 inCaps in a row means an acronym
        var inCaps = char.IsUpper(c);
        for (var i = 1; i < source.Length; i++)
        {
            c = source[i];
            if (char.IsUpper(c))
            {
                if (!inCaps)
                {
                    //looks like a new word so add space
                    builder.Append(' ');
                    inCaps = true;
                }
            }
            else
            {
                inCaps = false;
            }
            builder.Append(c);
        }
        return builder.ToString();
    }
}
