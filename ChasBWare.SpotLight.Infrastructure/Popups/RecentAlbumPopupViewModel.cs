using ChasBWare.SpotLight.Definitions.Enums;
using System.Xml.Linq;
using ChasBWare.SpotLight.Definitions.ViewModels;
using CommunityToolkit.Maui.Core;
using ChasBWare.SpotLight.Definitions.Tasks;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class RecentAlbumPopupViewModel(IPopupService popupService/*,
                                               IServiceProvider _serviceProvider*/)
                    : PopupMenuViewModel(popupService)
{
    public void SetItem(IRecentAlbumsViewModel viewModel, IPlaylistViewModel? item)
    {
        MenuGroups.Clear();

        AddItem($"{PopupAction.Clear}",
                caption: "Clear List",
                toolTip: "Clear all items from list",
                action: (t) =>
                {
                    //var task = _serviceProvider.GetRequiredService<IRemoveRecentArtistTask>();
                    //task.Execute(viewModel);
                    Close();
                });

        if (item != null)
        {
            AddItem(PopupGroup.Recent, $"{PopupAction.Delete}",
                    caption: $"Delete",
                    toolTip: "Delete item from list",
                    action: (t) =>
                    {
                      //  var task = _serviceProvider.GetRequiredService<IRemoveRecentArtistTask>();
                      //  task.Execute(viewModel, item);
                        Close();
                    });
        }
        Height = GetHeight();

    }
}

