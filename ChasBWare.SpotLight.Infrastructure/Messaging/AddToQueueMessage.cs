using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public readonly struct AddToQueueMessageArgs
    {
        public AddToQueueMessageArgs(ITrackViewModel track)
        {
            Playlist = null;
            Track = track;
        }

        public AddToQueueMessageArgs(IPlaylistViewModel playlist)
        {
            Playlist = playlist;
            Track = null;
        }

        public IPlaylistViewModel? Playlist { get; }
        public ITrackViewModel? Track { get; }
    }


    public class AddToQueueMessage
               : Message<AddToQueueMessageArgs>
    {
        public AddToQueueMessage(ITrackViewModel track) 
             : base(new AddToQueueMessageArgs(track)) 
        { }

        public AddToQueueMessage(IPlaylistViewModel playlist)
            : base(new AddToQueueMessageArgs(playlist))
        { }
    }

  
}
