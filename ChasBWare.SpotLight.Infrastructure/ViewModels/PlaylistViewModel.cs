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
        private bool _isExpanded = false;
        private RecentPlaylist _model = new() { Id = "" };
       
        public PlaylistViewModel(ITrackListViewModel tracksViewModel,
                                 IServiceProvider provider,
                                 IMessageService<PlayTracklistMessage> messageService)
        {
            TracksViewModel = tracksViewModel;
            _provider = provider;
            _messageService = messageService;
            SetExpandedCommand = new Command(() => IsExpanded = !IsExpanded);
            PlayTracklistCommand = new Command(PlayTrackList);
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

        public ICommand SetExpandedCommand { get; }
        public ICommand PlayTracklistCommand { get; } 
      
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

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (SetField(ref _isExpanded, value) &&
                    _isExpanded &&
                    TracksViewModel.LoadStatus == LoadState.NotLoaded)
                {
                    LoadTracks();
                }
            }
        }

        private void PlayTrackList()
        {
            IsExpanded = true;
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
