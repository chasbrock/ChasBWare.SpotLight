using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public class ActiveArtistChangedMessage(Artist payload) : Message<Artist>(payload)
    {
    }
}
