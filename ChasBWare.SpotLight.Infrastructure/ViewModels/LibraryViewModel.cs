using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public partial class LibraryViewModel 
                       : BaseGroupedListViewModel<IPlaylistViewModel>,
                         ILibraryViewModel
    {
        private bool _showOwner;

        public LibraryViewModel(IServiceProvider serviceProvider,
                                IPopupService popupService,       
                                IPlayerControlViewModel playerControlViewModel,
                                IMessageService<CurrentTrackChangedMessage> currentTrackChangedMessage)
             : base(serviceProvider, GrouperHelper.GetPlaylistGroupers())
        {
            currentTrackChangedMessage.Register(OnTrackChangedMessage);
            PlayerControlViewModel = playerControlViewModel;

            OpenPopupCommand = new Command<ITrackViewModel>(t => popupService.ShowPopup<LibraryPopupViewModel>());

            Initialise();
        }

        public IPlayerControlViewModel PlayerControlViewModel { get; }
        public ICommand OpenPopupCommand { get; }
        
        
        public void ExecuteLibrayCommand(IPlaylistViewModel? selectedItem)
        {
            if (selectedItem != null)
            {
                // var playlistService = _serviceProvider.GetService<IPlaylistService>();
                // playlistService.RemoveSavedItem(selectedItem.Model);
                Items.Remove(selectedItem);
            }
        }
            
        public bool ShowOwner
        {
            get => _showOwner;
            set => SetField(ref _showOwner, value);
        }
       
        public bool Exists(string? playlistId)
        {
            return !string.IsNullOrEmpty(playlistId) &&
                    Items.Any(pl => pl.Id == playlistId);
        }
        
        public override void RefreshView()
        {
            base.RefreshView();
            ShowOwner = SelectedGrouper.Name != nameof(IPlaylistViewModel.Owner);
        }

        protected override void SelectedItemChanged(IPlaylistViewModel? oldItem, IPlaylistViewModel? newItem)
        {
            if (oldItem != null)
            {
                oldItem.IsSelected = false;
            }

            if (newItem != null)
            {
                newItem.IsExpanded = true;
                newItem.IsSelected = true;
                newItem.LastAccessed = DateTime.Now;
                var task = _serviceProvider.GetService<IUpdateLastAccessedTask>();

                task?.Execute(newItem.Model);
            }
        }

        private void Initialise()
        {   
            var loadPlaylistsTask = _serviceProvider.GetRequiredService<ILibraryLoaderTask>();
            loadPlaylistsTask.Execute(this);
        }

        private void OnTrackChangedMessage(CurrentTrackChangedMessage message)
        {
            // if there is no playlist selected then try and find the one playing
            if (SelectedItem == null)
            {
                SelectedItem = Items.FirstOrDefault(pl => pl.Name.Equals(message.Payload.AlbumName, StringComparison.CurrentCultureIgnoreCase));
            }

            // try to find the track
            foreach (var playList in Items.Where(pl => pl.TracksViewModel.LoadStatus == LoadState.Loaded))
            {
                var track = playList.TracksViewModel.Items.FirstOrDefault(t => t.Id == message.Payload.TrackId);
                if (track != null)
                {
                    track.Status = message.Payload.State;
                }
            }
        }

     
    }
}
