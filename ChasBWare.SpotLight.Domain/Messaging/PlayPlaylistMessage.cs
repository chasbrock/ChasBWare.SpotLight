using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Domain.Messaging
{
    public class PlayPlaylistMessage(Playlist playlist, int offset)
               : Message()
    {
        public Playlist Playlist { get; } = playlist;
        public int Offset { get; } = offset;
    }
}
