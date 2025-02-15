using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class PlaylistViewModel : Notifyable, IPlaylistViewModel
    {
        private readonly IServiceProvider _provider;
        private readonly IMessageService<PlayPlaylistMessage> _messageService;
  
        private bool _isExpanded = false;
        private bool _isSelected = false;
        private Playlist _model = new() { Id = "" };
       
        public PlaylistViewModel(ITrackListViewModel tracksViewModel,
                                 IServiceProvider provider,
                                 IPopupService popupService,
                                 IMessageService<PlayPlaylistMessage> messageService)
        {
            TracksViewModel = tracksViewModel;
            _provider = provider;
            _messageService = messageService;
          
            SetExpandedCommand = new Command(() => IsExpanded = !IsExpanded);
            PlayTracklistCommand = new Command(PlayTrackList);
            OpenTrackPopupCommand = new Command<ITrackViewModel>(t => popupService.ShowPopup<TrackPopupViewModel>(onPresenting: vm => vm.SetTrack(this, TracksViewModel.SelectedItem)));
        }

        public Playlist Model 
        { 
            get => _model;
            set
            {
                _model = value;
                TracksViewModel.Playlist = this;
            }
        }

        public ICommand OpenTrackPopupCommand { get; }
        public ICommand SetExpandedCommand { get; }
        public ICommand PlayTracklistCommand { get; } 
        public ITrackListViewModel TracksViewModel { get; }

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

        public bool IsSelected
        {
            get => _isSelected;
            set => SetField(ref _isSelected, value);
        }

        private void PlayTrackList()
        {
            IsExpanded = true;
            _messageService.SendMessage(new PlayPlaylistMessage(this, 0));
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

        public bool InLibrary { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
