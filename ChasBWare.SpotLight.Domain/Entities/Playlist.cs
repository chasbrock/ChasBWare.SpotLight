using ChasBWare.SpotLight.Domain.Enums;
using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities
{
    public class Playlist 
    {
        [PrimaryKey, NotNull]
        public string? Id { get; set; }
        
        [NotNull]
        public string? Name { get; set; }
        public string? Owner { get; set; }
        public string? Description { get; set; } 
        public string? Image { get; set; }
        public PlaylistType PlaylistType { get; set; }
        public DateTime ReleaseDate { get; set; } = new DateTime(1900, 1, 1);
        public string Uri { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name ?? "err";
        }
    }
}
