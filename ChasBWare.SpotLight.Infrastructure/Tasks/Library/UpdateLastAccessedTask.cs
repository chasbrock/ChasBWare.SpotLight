using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Library;

public class UpdateLastAccessedTask(IServiceProvider _serviceProvider)
           : IUpdateLastAccessedTask
{
    public void Execute(Playlist playlist)
    {
        Task.Run(() =>
        {
            var repo = _serviceProvider.GetRequiredService<ILibraryRepository>();
            repo.UpdateLastAccessed(playlist);
        });
    }

    public void Execute(Artist artist)
    {
        Task.Run(() =>
        {
            var repo = _serviceProvider.GetRequiredService<IArtistRepository>();
            repo.UpdateLastAccessed(artist);
        });
    }

    public void Execute(User user)
    {
        Task.Run(() =>
        {
            var repo = _serviceProvider.GetRequiredService<IUserRepository>();
            repo.UpdateLastAccessed(user);
        });
    }
}
