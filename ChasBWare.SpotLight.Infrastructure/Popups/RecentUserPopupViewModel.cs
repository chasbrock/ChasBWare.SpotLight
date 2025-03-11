using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class RecentUserPopupViewModel(IPopupService popupService,
                                              IServiceProvider _serviceProvider)
                   : PopupMenuViewModel(popupService)
{
    public void SetItem(IRecentUserViewModel viewModel, IUserViewModel? item)
    {
        MenuGroups.Clear();

        AddItem(PopupActivity.Clear,
                caption: "Clear List",
                toolTip: "Clear all items from list",
                action: (t) =>
                {
                    var task = _serviceProvider.GetRequiredService<IRemoveRecentUserTask>();
                    task.Execute(viewModel);
                    Close();
                });

        if (item != null)
        {
            AddItem(PopupGroup.Recent,
                    PopupActivity.Delete,
                    caption: $"Delete",
                    toolTip: "Delete item from list",
                    action: (t) =>
                    {
                        var task = _serviceProvider.GetRequiredService<IRemoveRecentUserTask>();
                        task.Execute(viewModel, item);
                        Close();
                    });
        }
        RecalcSize();
    }


}



