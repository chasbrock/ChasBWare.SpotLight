using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{

    public readonly struct PlayTrackListMessageArgs(IPlaylistViewModel playlist, int offset)
    {
        public IPlaylistViewModel Playlist { get; } = playlist;
        public int Offset { get; } = offset;
    }

    public class PlayTracklistMessage(IPlaylistViewModel playlist, int offset)
               : Message<PlayTrackListMessageArgs>(new PlayTrackListMessageArgs(playlist, offset))
    {
    }
}
