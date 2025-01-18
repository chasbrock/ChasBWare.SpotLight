namespace ChasBWare.SpotLight.Infrastructure.Utility
{
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
    }
}
