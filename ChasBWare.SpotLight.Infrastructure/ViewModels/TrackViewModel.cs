using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public partial class TrackViewModel(IMessageService<PlayPlaylistMessage> _messageService)
                       : Notifyable, ITrackViewModel
    {
        private bool _isHated = false;
        public Track Track { get; set; } = new Track { Id = "" };

        public ICommand SetHatedTrackCommand { get; set; } = new Command<ITrackViewModel>(o => o.IsHated = !o.IsHated);
        public ICommand PlayTrackCommand { get; } = new Command<ITrackViewModel>(vm => vm.PlayTrackList());
        public IPlaylistViewModel? Playlist { get; set; }

       
        // [WriteableDataIndex(4)]
        public string Album
        {
            get => Track.Album;
        }

        // [WriteableDataIndex(5)]
        public string Artists
        {
            get => string.Join('|', Track.Artists);
        }

        // [WriteableDataIndex(3)]
        public string Duration
        {
            get => Track.Duration.MSecsToMinsSecs();
        }

        //[WriteableDataIndex(1)]
        public string Id
        {
            get => Track.Id??"";
        }

        //[WriteableDataIndex(2)]
        public string Name
        {
            get => Track.Name;
        }

        // [WriteableDataIndex(0)]
        public int TrackNumber
        {
            get => Track.TrackNumber;
        }

        public bool IsHated
        {
            get => _isHated;
            set => SetField(ref _isHated, value);
        }
                
        public TrackStatus Status
        {
            get => Track.Status;
            set
            {
                SetField(Track, value);
            }
        }

        public void PlayTrackList()
        {
            if (Playlist != null)
            {
                var offset = Playlist.TracksViewModel.Items.ToList().FindIndex(tm => tm.Id == Id);
                _messageService.SendMessage(new PlayPlaylistMessage(Playlist, offset));
            }
        }

        public override string ToString()
        {
            return Track.Name;
        }

    }
}
