namespace ChasBWare.SpotLight.Domain.Entities
{
    public class PlayingTrack
    {
        public required string TrackId { get; set; }
        public required Track Track { get; set; }
        public int ProgressMs { get; set; }
    }
}
