using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public partial class PlaylistSelectorViewModel(IServiceProvider serviceProvider)
                       : BaseGroupedListViewModel<IPlaylistViewModel>( serviceProvider,
                            GrouperHelper.GetPlaylistGroupers())
    {
        protected override void InitialiseSelectedItem(IPlaylistViewModel item)
        {

        }
    }
}
