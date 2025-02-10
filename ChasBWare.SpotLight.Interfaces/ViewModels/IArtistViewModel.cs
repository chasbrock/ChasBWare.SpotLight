using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IArtistViewModel: ISortedListViewModel<IPlaylistViewModel>
    {
        string Id { get; }
        string Name { get; }
        Artist Model { get; set; }
        string? Image { get; set; }
        DateTime LastAccessed { get; set; }
    }
}
