using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities;

public class ArtistPlaylist
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed(Name = "ArtistPlaylist_Key", Order = 0, Unique = true), NotNull]
    public string? ArtistId { get; set; }

    [Indexed(Name = "ArtistPlaylist_Key", Order = 1, Unique = true), NotNull]
    public string? PlaylistId { get; set; }
}
