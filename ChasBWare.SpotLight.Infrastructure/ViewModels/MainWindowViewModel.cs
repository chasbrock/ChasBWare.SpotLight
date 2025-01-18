using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class MainWindowViewModel : Notifyable, IMainWindowViewModel
    {
        public enum MainWindowTabPages
        {
            Library = 0, Artists = 1, Albums = 2, Playlists = 3, Queue = 4
        }

        // put a different value in so that when we set it during
        // constructor it will cause the SetField to fire and call initisialisation
        private int _selectedTabIndex = (int)MainWindowTabPages.Queue;

        public MainWindowViewModel(IRecentArtistsViewModel recentArtistViewModel,
                                   IRecentAlbumsViewModel recentAlbumsViewModel,
                                   IRecentPlaylistsViewModel recentPlaylistsViewModel,
                                   ILibraryViewModel libraryViewModel,
                                   IQueueViewModel queueViewModel,
                                   IPlayerControlViewModel playerControlViewModel)
        {
            RecentArtistsViewModel = recentArtistViewModel;
            RecentAlbumsViewModel = recentAlbumsViewModel;
            RecentPlaylistsViewModel = recentPlaylistsViewModel;
            LibraryViewModel = libraryViewModel;
            QueueViewModel = queueViewModel;
            PlayerControlViewModel = playerControlViewModel;

            SelectedTabIndex = (int)MainWindowTabPages.Library;

        }

        public IRecentArtistsViewModel RecentArtistsViewModel { get; private set; }
        public IRecentAlbumsViewModel RecentAlbumsViewModel { get; private set; }
        public IRecentPlaylistsViewModel RecentPlaylistsViewModel { get; private set; }
        public ILibraryViewModel LibraryViewModel { get; private set; }
        public IQueueViewModel QueueViewModel { get; private set; }
        public IPlayerControlViewModel PlayerControlViewModel { get; private set; }

        public MainWindowTabPages SelectedPage => (MainWindowTabPages)SelectedTabIndex;

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (SetField(ref _selectedTabIndex, value))
                {
                    InitialiseViewModel();
                };
            }
        }

        private void InitialiseViewModel()
        {
            switch (SelectedPage)
            {
                case MainWindowTabPages.Library:
                    LibraryViewModel.Initialise();
                    break;
                case MainWindowTabPages.Artists:
                    RecentArtistsViewModel.Initialise();
                    break;
                case MainWindowTabPages.Albums:
                    RecentAlbumsViewModel.Initialise();
                    break;
                case MainWindowTabPages.Playlists:
                    RecentPlaylistsViewModel.Initialise();
                    break;
                case MainWindowTabPages.Queue:
                    QueueViewModel.LoadQueue();
                    break;
            }
        }
    }
}
