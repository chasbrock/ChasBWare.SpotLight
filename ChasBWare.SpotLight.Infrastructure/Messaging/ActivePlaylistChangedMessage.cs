using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{

    public class ActivePlaylistChangedMessage(RecentPlaylist payload) : Message<RecentPlaylist>(payload)
    {
    }
}
