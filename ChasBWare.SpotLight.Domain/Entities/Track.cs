using ChasBWare.SpotLight.Domain.Enums;
using SQLite;


namespace ChasBWare.SpotLight.Domain.Entities
{
    public class Track
    {
        [PrimaryKey, NotNull]
        public string? Id { get; set; }
        public string Album { get; set; } = string.Empty;
        public string Artists { get; set; } = string.Empty;
        public int Duration { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public int TrackNumber { get; set; } = 0;
        public string Uri { get; set; } = string.Empty;
  
        [Ignore]
        public TrackStatus Status { get; set; } = TrackStatus.NotPlaying;

        public override string ToString()
        {
            return Name;
        }
    }
   
}
