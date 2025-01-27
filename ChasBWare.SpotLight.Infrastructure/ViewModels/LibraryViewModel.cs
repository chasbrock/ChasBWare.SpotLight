using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public partial class LibraryViewModel 
                       : BaseGroupedListViewModel<IPlaylistViewModel>,
                         ILibraryViewModel
    {
        public LibraryViewModel(IServiceProvider serviceProvider,
                                IMessageService<CurrentTrackChangedMessage> currentTrackChangedMessage)
             : base(serviceProvider, GrouperHelper.GetPlaylistGroupers())
        {
            currentTrackChangedMessage.Register(OnTrackChangedMessage);
            Initialise();
        }


        public void ExecuteLibrayCommand(IPlaylistViewModel? selectedItem)
        {
            if (selectedItem != null)
            {
                // var playlistService = _serviceProvider.GetService<IPlaylistService>();
                // playlistService.RemoveSavedItem(selectedItem.Model);
                Items.Remove(selectedItem);
            }
        }

        public void Initialise()
        {
            if (LoadStatus != LoadState.NotLoaded)
            {
                return;
            }

            var loadPlaylistsTask = _serviceProvider.GetService<ILibraryLoaderTask>();
            loadPlaylistsTask?.Execute(this);
        }

        protected override void InitialiseSelectedItem(IPlaylistViewModel item) 
        {
            item.IsTracksExpanded = true;
            item.LastAccessed = DateTime.Now;
            var task = _serviceProvider.GetService<IUpdateLastAccessedTask>();

            task?.Execute(item.Id, item.LastAccessed, true);
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
