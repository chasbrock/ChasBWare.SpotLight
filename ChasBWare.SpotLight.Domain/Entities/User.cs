using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities;

public class User
{
    [PrimaryKey, NotNull]
    public string? Id { get; set; }
    public DateTime LastAccessed { get; set; }

    [NotNull]
    public string Name { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
    public string Country { get; set; } = "GB";
    public string? Image { get; set; }
}
