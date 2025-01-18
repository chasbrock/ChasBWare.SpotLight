using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities
{
    /// <summary>
    /// holds reference to items (Artist / playlist) when they
    /// have been recently accessed via a search screen
    /// isSaved meands that they are part of users saved library
    /// </summary>
    public class RecentItem
    {   
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Name = "RecentItem_Key", Order = 0, Unique = true), NotNull]
        public string? UserId { get; set; }

        [Indexed(Name = "RecentItem_Key", Order = 1, Unique = true), NotNull]
        public string? ItemId { get; set; }

        public bool IsSaved { get; set; } = false;
        public DateTime LastAccessed { get; set; }
    }
}
