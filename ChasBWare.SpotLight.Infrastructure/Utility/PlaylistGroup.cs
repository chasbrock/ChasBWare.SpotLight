using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    public partial class PlaylistGroup : GroupHolder<IPlaylistViewModel>
    {
        public PlaylistGroup(object key, List<IPlaylistViewModel> items, bool hideOwner = false)
             : base(key, items)
        {
            IsExpanded = false;
            // do not show owner if this is a playlist and we are grouping on PlaylistType

            if (key is PlaylistType)
            {
                ShowOwner = items[0].PlaylistType == PlaylistType.Album;
            }
            else
            {
                ShowOwner = !hideOwner;
            }
        }

        public bool ShowOwner { get; }
    }
}
