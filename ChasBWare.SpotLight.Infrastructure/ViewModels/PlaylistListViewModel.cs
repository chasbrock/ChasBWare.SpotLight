using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class PlaylistListViewModel
     : BaseSortedListViewModel<IPlaylistViewModel>,
       IPlaylistListViewModel
{
    public PlaylistListViewModel(IServiceProvider serviceProvider)
         : base(serviceProvider, SorterHelper.GetPlaylistListSorters())
    {
    }

    protected override void SelectedItemChanged(IPlaylistViewModel? oldItem, IPlaylistViewModel? newItem)
    {
        if (oldItem != null)
        {
            oldItem.IsSelected = false;
        }
        if (newItem != null)
        {
            newItem.IsSelected = true;
        }
    }
}
