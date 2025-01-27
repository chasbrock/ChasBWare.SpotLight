using System.Windows.Input;

using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{


    public class PlaylistViewModel : Notifyable, IPlaylistViewModel
    {
        private readonly IServiceProvider _provider;
        private readonly IMessageService<PlayTracklistMessage> _messageService;
        private bool _isTracksExpanded = false;
        private RecentPlaylist _model = new() { Id = "" };
       
        public PlaylistViewModel(ITrackListViewModel tracksViewModel,
                                 IServiceProvider provider,
                                 IMessageService<PlayTracklistMessage> messageService)
        {
            TracksViewModel = tracksViewModel;
            _provider = provider;
            _messageService = messageService;
        }

        public RecentPlaylist Model 
       { 
            get => _model;
            set
            {
                _model = value;
                TracksViewModel.PlaylistName = value.Name ?? "";
            }
        } 
        
        public ICommand PlayTracklistCommand { get; } = new Command<PlaylistViewModel>(vm => vm.PlayTrackList());
        public ICommand AddToQueueCommand { get; } = new Command<PlaylistViewModel>(vm => vm.PlayTrackList());

        public ITrackListViewModel TracksViewModel
        {
            get;
            private set;
        }

        public string Description
        {
            get => Model.Description ?? "";
            set => SetField(Model, value);
        }

        public string Id
        {
            get => Model.Id ?? "";
        }

        public string? Image
        {
            get => Model.Image;
        }

        public bool IsTracksExpanded
        {
            get => _isTracksExpanded;
            set
            {
                if (SetField(ref _isTracksExpanded, value) &&
                    _isTracksExpanded &&
                    TracksViewModel.LoadStatus == LoadState.NotLoaded)
                {
                    LoadTracks();
                }
            }
        }

        private void PlayTrackList()
        {
            IsTracksExpanded = true;
            _messageService.SendMessage(new PlayTracklistMessage(this, 0));
        }

        private void LoadTracks()
        {
            if (TracksViewModel.LoadStatus == LoadState.NotLoaded)
            {
                TracksViewModel.LoadStatus = LoadState.Loading;
                var task = _provider.GetService<ITrackListLoaderTask>();
                task?.Execute(this);
            }
        }

        public string Name
        {
            get => Model.Name??"";
        }

        public string Owner
        {
            get => Model.Owner?? "";
        }

        public PlaylistType PlaylistType
        {
            get => Model.PlaylistType;
        }

        public DateTime ReleaseDate
        {
            get => Model.ReleaseDate;
        }

        public string Uri
        {
            get => Model.Uri;
        }

        public DateTime LastAccessed
        {
            get => Model.LastAccessed;
            set => SetField(Model, value); 

        }

        public override string ToString()
        {
            return Name;
        }
    }
}
