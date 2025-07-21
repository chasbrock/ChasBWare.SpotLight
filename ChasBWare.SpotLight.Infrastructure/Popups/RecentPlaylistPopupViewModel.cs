using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui;
using ChasBWare.SpotLight.Infrastructure.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class RecentPlaylistPopupViewModel(IPopupService popupService,
                                               IServiceProvider _serviceProvider)
                    : PopupMenuViewModel(popupService)
{
    public const string ViewModelName = "ViewModel";
    public const string ItemName = "Item";

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!(query[ViewModelName] is IRecentViewModel<IPlaylistViewModel> viewModel))
        {
            return;
        }
        var item = query[ViewModelName] as IPlaylistViewModel;

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

    public static IDictionary<string, object>? BuildParams(IRecentViewModel<IPlaylistViewModel> recentAlbumsViewModel,
                                                           IPlaylistViewModel? selectedItem)
    {
        var paramList = new Dictionary<string, object> { { ViewModelName, recentAlbumsViewModel } };
        if (selectedItem != null)
        {
            paramList.Add(ItemName, selectedItem);
        }
        return paramList;
    }

}

