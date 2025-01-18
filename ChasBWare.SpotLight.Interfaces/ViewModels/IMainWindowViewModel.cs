namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IMainWindowViewModel 
    {
        IRecentArtistsViewModel RecentArtistsViewModel { get; }
        IRecentAlbumsViewModel RecentAlbumsViewModel { get; }
        IRecentPlaylistsViewModel RecentPlaylistsViewModel { get; }
        ILibraryViewModel LibraryViewModel { get; }
        IQueueViewModel QueueViewModel { get; }
        IPlayerControlViewModel PlayerControlViewModel { get; }
    }
}
