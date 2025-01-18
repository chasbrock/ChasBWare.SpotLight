using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities
{
    public class UserPlaylist
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Name = "UserPlaylist_Key", Order = 0, Unique = true), NotNull]
        public string? UserId { get; set; }

        [Indexed(Name = "UserPlaylist_Key", Order = 1, Unique = true), NotNull]
        public string? PlaylistId { get; set; }
    }



}
