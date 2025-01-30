using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Interfaces.Services
{
    public readonly struct TrackProgressMessageArgs(string trackName, string artist, 
                                                    int progressPercent, string progressText, 
                                                    TrackStatus status)
    {
        public string TrackName { get; } = trackName;
        public string Artist { get; } = artist;
        public int ProgressPercent { get; } = progressPercent;
        public string ProgressText { get; } = progressText;
        public TrackStatus Status { get; } = status;
    }

}
