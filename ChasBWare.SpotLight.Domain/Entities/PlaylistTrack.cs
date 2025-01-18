using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities
{
    public class PlaylistTrack
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Name = "PlaylistTrack_Key", Order = 0, Unique = true), NotNull]
        public string? PlaylistId { get; set; }
       
        [Indexed(Name = "PlaylistTrack_Key", Order = 1, Unique = true), NotNull]
        public string? TrackId { get; set; }

        public int TrackOrder { get; set; }
    }
}
