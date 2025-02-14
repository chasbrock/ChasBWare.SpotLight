using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities
{
    public class SearchItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string? ItemId { get; set; }
    }

}
