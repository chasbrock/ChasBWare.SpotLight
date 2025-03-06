using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities;

/// <summary>
/// holds reference to items (Artist / playlist) that you never
/// want to hear
/// </summary>
public class HatedItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed(Name = "HatedItem_Key", Order = 1, Unique = true), NotNull]
    public string? ItemId { get; set; }
}
