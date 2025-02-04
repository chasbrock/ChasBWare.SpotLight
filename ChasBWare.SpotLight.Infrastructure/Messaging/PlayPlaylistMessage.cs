using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public readonly struct PlayPlaylistMessageArgs(IPlaylistViewModel playlist, int offset)
    {
        public IPlaylistViewModel Playlist { get; } = playlist;
        public int Offset { get; } = offset;
    }

    public class PlayPlaylistMessage(IPlaylistViewModel playlist, int offset)
               : Message<PlayPlaylistMessageArgs>(new PlayPlaylistMessageArgs(playlist, offset))
    {
    }
}
