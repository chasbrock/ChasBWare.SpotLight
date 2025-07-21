using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Utility;

public static class PlaylistClipboardHelper
{
    public static string WriteTrack(this Track track)
    {
        return $"{track.Id}\t{track.Name}\t{track.Duration.MSecsToMinsSecs()}\t{track.Album}\t{track.Artists.RepackOwners(';')}\t{track.Uri}";
    }


    public static void ParseTracks(this List<Track> list, string text) 
    {
        if (string.IsNullOrWhiteSpace(text)) 
        {
            return;
        }

        foreach (var line in text.Split('\n', StringSplitOptions.RemoveEmptyEntries |
                                             StringSplitOptions.TrimEntries))
        {
            var items = line.Split('\t', StringSplitOptions.TrimEntries);
            if (items.Length == 0)
            {
                continue;
            }
            var track = new Track { Id = items[0] };
            if (items.Length > 1)
            {
                track.Name = items[1];
            }
            if (items.Length > 2)
            {
                track.Duration = items[2].HoursMinsSecsToMs();
            }
            if (items.Length > 3)
            {
                track.Album = items[3];
            }
            if (items.Length > 4)
            {
                track.Artists = items[4];
            }
            if (items.Length > 5)
            {
                track.Uri = items[5];
            }
            list.Add(track);
        }
    }
}
