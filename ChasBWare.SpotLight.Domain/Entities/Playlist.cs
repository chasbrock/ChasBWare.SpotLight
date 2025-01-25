using ChasBWare.SpotLight.Domain.Enums;
using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities
{
    public class Playlist 
    {
        private DateTime releaseDate;

        [PrimaryKey, NotNull]
        public string? Id { get; set; }
        
        [NotNull]
        public string? Name { get; set; }
        public string? Owner { get; set; }
        public string? Description { get; set; } 
        public string? Image { get; set; }
        public PlaylistType PlaylistType { get; set; }
        public DateTime ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public string Uri { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name ?? "err";
        }
    }

    public class RecentPlaylist : Playlist
    {
        public DateTime LastAccessed { get; set; }
    }


    public static class PlaylistHelper
    {
        public static Playlist ToPlaylist(this RecentPlaylist source)
        {
            return new Playlist
            {
                Id = source.Id,
                Description = source.Description,
                Image = source.Image,
                Name = source.Name,
                Owner = source.Owner,
                PlaylistType = source.PlaylistType,
                ReleaseDate = source.ReleaseDate,
                Uri = source.Uri
            };
        }
    }
}
