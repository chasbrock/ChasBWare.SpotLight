using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public class ActiveAlbumChangedMessage(RecentPlaylist payload) : Message<RecentPlaylist>(payload)
    {
    }
}
