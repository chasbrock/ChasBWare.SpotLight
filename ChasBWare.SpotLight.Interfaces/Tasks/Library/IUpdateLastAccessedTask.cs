using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Tasks.Library;

public interface IUpdateLastAccessedTask
{
    void Execute(Playlist playlist);
    void Execute(Artist artist);
    void Execute(User user);
}
