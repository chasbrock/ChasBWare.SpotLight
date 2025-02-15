using ChasBWare.SpotLight.Definitions.Enums;
using System.Xml.Linq;
using CommunityToolkit.Maui.Core;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class RecentPlaylistPopupViewModel(IPopupService popupService,
                                               IServiceProvider _serviceProvider)
                    : PopupMenuViewModel(popupService)
{
    public void SetItem(IRecentViewModel<IPlaylistViewModel> viewModel, IPlaylistViewModel? item)
    {
        MenuGroups.Clear();

        AddItem(PopupActivity.Clear,
                caption: "Clear List",
                toolTip: "Clear all items from list",
                action: (t) =>
                {
                    var task = _serviceProvider.GetRequiredService<IRemovePlaylistTask>();
                    task.Execute(viewModel);
                    Close();
                });

        if (item != null)
        {
            AddItem(PopupGroup.Recent, 
                    PopupActivity.Delete,
                    caption: $"Delete '{item.Name}'",
                    toolTip: "Delete item from list",
                    action: (t) =>
                    {
                        var task = _serviceProvider.GetRequiredService<IRemovePlaylistTask>();
                        task.Execute(viewModel, item);
                        Close();
                    });
        }
        RecalcSize();

    }
}

