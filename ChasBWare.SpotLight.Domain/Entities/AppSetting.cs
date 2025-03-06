using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities;

public class AppSetting
{
    [PrimaryKey, NotNull]
    public string? Name { get; set; }

    [NotNull]
    public string? Value { get; set; }
}
