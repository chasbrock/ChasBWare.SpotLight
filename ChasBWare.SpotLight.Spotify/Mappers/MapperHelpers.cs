using System.Text;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Mappings.Mappers;

public static class MapperHelpers
{
    public static DateTime ConvertReleaseDate(this string? releaseDate)
    {
        var year = 1900;
        var month = 1;
        var day = 1;

        if (!string.IsNullOrWhiteSpace(releaseDate))
        {
            var parts = releaseDate.Trim().Split('-');
            if (parts.Length >= 1)
            {
                year = int.Parse(parts[0]);
            }

            if (parts.Length >= 2)
            {
                month = int.Parse(parts[1]);
            }

            if (parts.Length >= 3)
            {
                day = int.Parse(parts[2]);
            }
        }

        return new DateTime(year, month, day);
    }

    public static string? GetMediumImage(this List<SpotifyAPI.Web.Image>? images)
    {
        if (images == null || images.Count == 0)
        {
            return null;
        }

        var sorted = images.OrderByDescending(i => i.Width).ToArray();
        if (sorted.Length > 1)
        {
            return sorted[1].Url;
        }

        return sorted[0].Url;
    }

    public static string? GetSmallImage(this List<SpotifyAPI.Web.Image>? images)
    {
        if (images == null || images.Count == 0)
        {
            return null;
        }

        var smallest = images.OrderBy(i => i.Width).FirstOrDefault();

        return smallest?.Url;
    }

    public static string PackOwner(this List<SimpleArtist>? source)
    {
        if (source == null)
        {
            return string.Empty;
        }

        var result = new StringBuilder();
        source.ForEach(sa => result.Append($"{sa.PackOwner()},"));
        return result.ToString();
    }

    public static string PackOwner(this PublicUser? source)
    {
        if (source == null)
        {
            return string.Empty;
        }

        return $"{source.DisplayName}={source.Id}";
    }

    public static string PackOwner(this SimpleArtist source)
    {
        return $"{source.Name}={source.Id}";
    }
}
