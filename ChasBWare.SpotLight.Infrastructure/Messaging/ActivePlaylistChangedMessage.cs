using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{

    public class ActivePlaylistChangedMessage(Playlist payload) : Message<Playlist>(payload)
    {
    }
}
