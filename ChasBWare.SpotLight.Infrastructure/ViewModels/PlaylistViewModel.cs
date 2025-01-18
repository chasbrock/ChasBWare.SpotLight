

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
        private bool _isTracksExpanded = true;
        private Playlist _model = new() { Id = "" };
        private DateTime _lastAccessed;

        public PlaylistViewModel(ITrackListViewModel tracksViewModel,
                                 IServiceProvider provider,
                                 IMessageService<PlayTracklistMessage> messageService)
        {
            TracksViewModel = tracksViewModel;
            _provider = provider;
            _messageService = messageService;
        }

        public Playlist Model 
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
            set => SetField(Model, value);
        }

        public string? Image
        {
            get => Model.Image;
            set => SetField(Model, value);
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
            set => SetField(Model, value);
        }

        public string Owner
        {
            get => Model.Owner?? "";
            set => SetField(Model, value);
        }

        public PlaylistType PlaylistType
        {
            get => Model.PlaylistType;
            set => SetField(Model, value);
        }

        public DateTime ReleaseDate
        {
            get => Model.ReleaseDate;
            set => SetField(Model, value);
        }

        public string Uri
        {
            get => Model.Uri;
            set => SetField(Model, value);
        }

        public DateTime LastAccessed
        {
            get => _lastAccessed;
            set => SetField(ref _lastAccessed, value); 

        }
    }
}
