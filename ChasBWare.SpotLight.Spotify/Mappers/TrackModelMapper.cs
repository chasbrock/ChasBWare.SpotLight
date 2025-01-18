using ChasBWare.SpotLight.Domain.Entities;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Mappings.Mappers
{
    public static class TrackModelMapper
    {
        public static Track CopyToTrack(this FullTrack source)
        {
            return new Track
            {
                Id = source.Id,
                Name = source.Name,
                Album = source.Album.Name,
                Artists = string.Join('|', source.Artists.Select(a => a.Name)),
                Duration = source.DurationMs,
                TrackNumber = source.TrackNumber,
                Uri = source.Uri
            };
        }

        public static Track CopyToTrack(this SimpleTrack source)
        {
            return new Track
            {
                Id = source.Id,
                Name = source.Name,
                Album = string.Empty,
                Artists = string.Join('|', source.Artists.Select(a => a.Name)),
                Duration = source.DurationMs,
                TrackNumber = source.TrackNumber,
                Uri = source.Uri
            };
        }

        public static Track CopyToTrack(this IPlayableItem source) 
        {
            if (source is SimpleTrack simpleTrack)
            {
                return CopyToTrack(simpleTrack);
            }
            if (source is FullTrack fullTrack)
            {
                return CopyToTrack(fullTrack);
            }
            throw new NotImplementedException($"No convertion implemented for converting '{source?.GetType()}' to Track");
        }
    }
}
