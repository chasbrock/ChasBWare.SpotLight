using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    public partial class PlaylistGroup : GroupHolder<IPlaylistViewModel>
    {
        public PlaylistGroup(IGroupedListViewModel<IPlaylistViewModel> owner, object key, List<IPlaylistViewModel> items)
             : base(owner, key, items)
        {
            IsExpanded = false;
        }

       
    }


}
