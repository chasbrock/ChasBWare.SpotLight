using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.Popups;

public partial class RecentArtistPopupViewModel(IPopupService popupService,
                                               IServiceProvider _serviceProvider)
                   : PopupMenuViewModel(popupService)
{
    public const string RecentArtistName = "RecentArtist";
    public const string ItemName = "ItemName";

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        object? obj = null;
        MenuGroups.Clear();
        if ((query.TryGetValue(RecentArtistName, out obj) && obj is IRecentArtistsViewModel artist))
        {
            AddItem(PopupActivity.Clear,
                  caption: "Clear List",
                  toolTip: "Clear all items from list",
                  action: (t) =>
                  {
                      var task = _serviceProvider.GetRequiredService<IRemoveRecentArtistTask>();
                      task.Execute(artist);
                      Close();
                  });
           
            if ((query.TryGetValue(ItemName, out obj) && obj is IArtistViewModel item))
            {
                AddItem(PopupGroup.Recent,
                        PopupActivity.Delete,
                        caption: $"Delete",
                        toolTip: "Delete item from list",
                        action: (t) =>
                        {
                            var task = _serviceProvider.GetRequiredService<IRemoveRecentArtistTask>();
                            task.Execute(artist, item);
                            Close();
                        });
            }
            RecalcSize();
        }
    }

    /// <summary>
    /// correctly builds params for show popup call
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static IDictionary<string, object> BuildParams(IRecentArtistsViewModel viewModel,
                                                          IArtistViewModel? item)
    {
        var paramList = new Dictionary<string, object>() {{ RecentArtistName, viewModel }};
        if (item != null)
        {
            paramList.Add(ItemName, item);
        }
        return paramList;
    }

}
