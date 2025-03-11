using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IUserViewModel : ISortedListViewModel<IPlaylistViewModel>
    {
        string Id { get; }
        string Name { get; }
        User Model { get; set; }
        string? Image { get; }
        DateTime LastAccessed { get; set; }
    }
}
