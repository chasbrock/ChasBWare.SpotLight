using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities
{
    /// <summary>
    /// holds artist selected during search
    /// </summary>
    public class Artist 
    {
        [PrimaryKey, NotNull]
        public string? Id { get; set; }
        public DateTime LastAccessed { get; set; }

        [NotNull]
        public string? Name { get; set; }
        public string? Image { get; set; }
    }
}
