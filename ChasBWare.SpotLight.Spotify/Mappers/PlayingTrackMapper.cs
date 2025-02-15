using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Mappings.Mappers
{
    public static class PlayingTrackMapper
    {
        public static PlayingTrack? CopyToPlayingTrack(this CurrentlyPlaying currentlyPlaying)
        {
            if (!(currentlyPlaying?.Item is FullTrack track))
            {
                return null;
            }

            return new PlayingTrack
            {
                Id = track.Id,
                Name = track.Name,
                Artists = track.Artists.Select(a => new KeyValue { Key = a.Id, Value = a.Name }).ToList(),
                Album = track.Album.Name,
                AlbumId = track.Album.Id,
                Image = track.Album.Images.GetSmallImage(),
                Uri = track.Uri,
                Progress = TimeSpan.FromMilliseconds(currentlyPlaying.ProgressMs??0),
                Duration = TimeSpan.FromMilliseconds(track.DurationMs),
                IsPlaying = currentlyPlaying.IsPlaying 
            };
        }

        public static PlayingTrack CopyToPlayingTrack(this CurrentlyPlayingContext currentlyPlaying)
        {
            if (!(currentlyPlaying?.Item is FullTrack track))
            {
                return Empty;
            }

            return new PlayingTrack
            {
                Id = track.Id,
                Name = track.Name,
                Artists = track.Artists.Select(a => new KeyValue { Key = a.Id, Value = a.Name }).ToList(),
                Album = track.Album.Name,
                AlbumId = track.Album.Id,
                Image = track.Album.Images.GetSmallImage(),
                Uri = track.Uri,
                Progress = TimeSpan.FromMilliseconds(currentlyPlaying.ProgressMs),
                Duration = TimeSpan.FromMilliseconds(track.DurationMs),
                IsPlaying = currentlyPlaying.IsPlaying
            };
        }


        public static readonly PlayingTrack Empty = new PlayingTrack { Id = "", Name = "", Artists = [], Uri = "", Album = "",AlbumId=""};
    }
}
