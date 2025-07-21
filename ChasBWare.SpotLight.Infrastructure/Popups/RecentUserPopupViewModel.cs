using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Infrastructure.ViewModels;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class RecentUserPopupViewModel(IPopupService popupService,
                                              IServiceProvider _serviceProvider)
                   : PopupMenuViewModel(popupService)
{
    public const string RecentUserName = "RecentUser";
    public const string UserName = "User";

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        MenuGroups.Clear();
        object? obj = null;
        if ((query.TryGetValue(RecentUserName, out obj) && obj is IRecentUserViewModel recentUser))
        {
            AddItem(PopupActivity.Clear,
                caption: "Clear List",
                toolTip: "Clear all items from list",
                action: (t) =>
                {
                    var task = _serviceProvider.GetRequiredService<IRemoveRecentUserTask>();
                    task.Execute(recentUser);
                    Close();
                });

            if (query[UserName] is IUserViewModel user)
            {
                AddItem(PopupGroup.Recent,
                        PopupActivity.Delete,
                        caption: $"Delete",
                        toolTip: "Delete item from list",
                        action: (t) =>
                        {
                            var task = _serviceProvider.GetRequiredService<IRemoveRecentUserTask>();
                            task.Execute(recentUser, user);
                            Close();
                        });
            }
            RecalcSize();
        }
    }

    public static IDictionary<string, object> BuildParams(RecentUserViewModel recentUserViewModel, IUserViewModel? selectedItem)
    {
        var paramList = new Dictionary<string, object>() { { RecentUserName, recentUserViewModel } };
        if (selectedItem != null)
        {
            paramList.Add(UserName, selectedItem);
        }
        return paramList;
    }

}



