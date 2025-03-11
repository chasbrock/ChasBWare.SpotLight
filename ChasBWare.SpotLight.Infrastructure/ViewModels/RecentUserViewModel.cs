using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;
using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class RecentUserViewModel
           : BaseRecentViewModel<IUserViewModel>,
             IRecentUserViewModel
{

    public RecentUserViewModel(IPopupService popupService,
                               IServiceProvider serviceProvider,
                               ISearchUserViewModel searchViewModel,
                               IPlayerControlViewModel playerControlViewModel,
                               IMessageService<FindItemMessage> findItemMessageService,
                               IMessageService<ActiveItemChangedMessage> activeItemChangedMessageService,
                               IMessageService<CurrentTrackChangedMessage> currentTrackChangedMessage)
             : base(popupService, serviceProvider, playerControlViewModel, searchViewModel, SorterHelper.GetUserSorters())
    {
        findItemMessageService.Register(OnFindItem);
        activeItemChangedMessageService.Register(OnSetActiveItem);
        currentTrackChangedMessage.Register(OnTrackChangedMessage);
    }

    public override PageType PageType => PageType.Artists;

    protected override void LoadItems()
    {
        var task = _serviceProvider.GetService<ILoadRecentUserTask>();
        task?.Execute(this);
    }

    protected override void SelectedItemChanged(IUserViewModel? oldItem, IUserViewModel? newItem)
    {
        if (newItem != null)
        {
            LoadItem(newItem);
            newItem.SelectedItem?.ShowPlayingTrack(PlayerControlViewModel.CurrentTrackId, PlayerControlViewModel.TrackStatus);
        }
    }

    protected override void OpenPopup()
    {
        _popupService.ShowPopup<RecentUserPopupViewModel>(onPresenting: vm => vm.SetItem(this, SelectedItem));
    }

    private IUserViewModel? AddItemToList(User user, DateTime lastAccessed)
    {
        var viewModel = Items.FirstOrDefault(a => a.Model.Id == user.Id);
        if (viewModel == null)
        {
            var task = _serviceProvider.GetRequiredService<IAddRecentUserTask>();
            task.Execute(this, user);
        }
        else
        {
            var task = _serviceProvider.GetRequiredService<IUpdateLastAccessedTask>();
            task.Execute(viewModel.Model);
        }

        return viewModel;
    }

    private void LoadItem(IUserViewModel item)
    {
        if (item!.LoadStatus == LoadState.NotLoaded)
        {
            item!.LoadStatus = LoadState.Loading;
            var task = _serviceProvider.GetService<IUserAlbumsLoaderTask>();
            task?.Execute(item);
        }
    }

    private void OnFindItem(FindItemMessage message)
    {
        if (message.PageType == PageType.Users)
        {
            var viewModel = Items.FirstOrDefault(a => a.Id == message.Id);
            if (viewModel != null)
            {
                viewModel.LastAccessed = DateTime.Now;
                SelectedItem = viewModel;
                return;
            }

            var task = _serviceProvider.GetRequiredService<IFindUserTask>();
            task.Execute(this, message.Id);
        }
    }

    private void OnSetActiveItem(ActiveItemChangedMessage message)
    {
        if (message.PageType == PageType.Users)
        {
            if (message.Model is User user)
            {

                SelectedItem = AddItemToList(user, DateTime.Now);
                if (SelectedItem != null)
                {
                    LoadItem(SelectedItem);
                }
            }
            else
            {
                SelectedItem = null;
            }
        }
        RefreshView();
    }

    private void OnTrackChangedMessage(CurrentTrackChangedMessage message)
    {
       SelectedItem?.SelectedItem?.ShowPlayingTrack(message.TrackId, message.State);
    }
}
