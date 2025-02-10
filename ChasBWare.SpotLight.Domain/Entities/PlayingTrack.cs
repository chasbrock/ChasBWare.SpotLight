using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Domain.Entities
{

    public class PlayingTrack 
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required List<KeyValue> Artists { get; set; }
        public required string Album { get; set; }
        public required string AlbumId { get; set; }
        public string? Image { get; set; }
        public required string Uri { get; set; }
        public TimeSpan Progress { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsPlaying { get; set; }
    }
}
