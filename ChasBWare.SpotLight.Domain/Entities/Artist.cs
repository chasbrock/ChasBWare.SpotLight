using SQLite;

namespace ChasBWare.SpotLight.Domain.Entities
{
    /// <summary>
    /// class to map onto Spotify artist classes
    /// </summary>
    public class Artist 
    {
        [PrimaryKey, NotNull]
        public string? Id { get; set; }
       
        [NotNull]
        public string? Name { get; set; }
        public string? Image { get; set; }
      
        
        public override string ToString()
        {
            return Name ?? "err";
        }
    }
}
