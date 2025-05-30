using System.Text;
using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities;

/// <summary>
/// holds artist selected during search
/// </summary>
public class Artist
{
    [PrimaryKey, NotNull]
    public string? Id { get; set; }
    public DateTime LastAccessed { get; set; }

    [NotNull]
    public string? Name { get; set; }
    public string? Image { get; set; }
}

public static class ArtistHelper
{
    public static List<KeyValue> UnpackOwners(this string? source)
    {
        List<KeyValue> list = [];
        foreach (var part in source?.Split('|') ?? [])
        {
            if (string.IsNullOrEmpty(source))
            {
                continue;
            }

            var sub = part.Split('=');
            if (sub.Length == 1 && sub[0] != null)
            {
                list.Add(new KeyValue { Key = sub[0], Value = string.Empty });
                continue;
            }

            if (sub.Length == 2 && sub[0] != null && sub[1] != null)
            {
                list.Add(new KeyValue { Key = sub[0], Value = sub[1] });
                continue;
            }
        }
        return list;
    }

    public static string RepackOwners(this string? source, char delim=';')
    {
        StringBuilder list = new();
        bool first = true;
        foreach (var part in source?.Split('|') ?? [])
        {
            if (string.IsNullOrEmpty(source))
            {
                continue;
            }

            var sub = part.Split('=');
            if (sub.Length >= 1 && sub[0].Length>0)
            {
                if (!first)
                {
                    list.Append(delim);
                }
                else
                {
                    first = false;
                }
                list.Append(sub[0]);
            }
        }
        return list.ToString();
    }


    public static string PackOwner(this Artist source)
    {
        return $"{source.Name}={source.Id}";
    }
}
