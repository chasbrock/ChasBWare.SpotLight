using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    public partial class PlaylistGroup : GroupHolder<IPlaylistViewModel>
    {
        public PlaylistGroup(object key, List<IPlaylistViewModel> items)
             : base(key, items)
        {
            IsExpanded = false;
        }

       
    }


}
