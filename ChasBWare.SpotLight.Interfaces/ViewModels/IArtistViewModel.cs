using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IArtistViewModel: ISortedListViewModel<IPlaylistViewModel>
    {
        Artist Model { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        string? Image { get; set; }
        DateTime LastAccessed { get; set; }
    }
}
