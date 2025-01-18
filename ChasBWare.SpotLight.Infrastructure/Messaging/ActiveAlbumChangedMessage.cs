using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public class ActiveAlbumChangedMessage(Playlist payload) : Message<Playlist>(payload)
    {
    }
}
