namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface ILibraryViewModel : IPlaylistSelectorViewModel
    {
        bool Exists(string? playlistId);
    }
}
