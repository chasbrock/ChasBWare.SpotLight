using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities;

public class OwnedPlaylist
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed(Name = "OwnedPlaylist_Key", Order = 0, Unique = true), NotNull]
    public string? OwnerId { get; set; }

    [Indexed(Name = "OwnedPlaylist_Key", Order = 1, Unique = true), NotNull]
    public string? PlaylistId { get; set; }
}

