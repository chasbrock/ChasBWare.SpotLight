using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class RecentArtistsViewModel
           : BaseRecentViewModel<IArtistViewModel>,
             IRecentArtistsViewModel
{
    public RecentArtistsViewModel(IPopupService popupService,
                                  IServiceProvider serviceProvider,
                                  ISearchArtistsViewModel searchViewModel,
                                  IPlayerControlViewModel playerControlViewModel,
                                  IMessageService<FindItemMessage> findItemMessageService,
                                  IMessageService<ActiveItemChangedMessage> activeItemChangedMessageService,
                                  IMessageService<CurrentTrackChangedMessage> currentTrackChangedMessage)
         : base(popupService, serviceProvider, playerControlViewModel, searchViewModel, SorterHelper.GetArtistSorters())
    {
        findItemMessageService.Register(OnFindItem);
        activeItemChangedMessageService.Register(OnSetActiveArtist);
        currentTrackChangedMessage.Register(OnTrackChangedMessage);
    }

    public override PageType PageType => PageType.Artists;

    protected override void LoadItems()
    {
        var task = _serviceProvider.GetService<ILoadRecentArtistTask>();
        task?.Execute(this);
    }

    protected override void SelectedItemChanged(IArtistViewModel? oldItem, IArtistViewModel? newItem)
    {
        if (newItem != null)
        {
            LoadItem(newItem);
            newItem.SelectedItem?.ShowPlayingTrack(PlayerControlViewModel.CurrentTrackId, PlayerControlViewModel.TrackStatus);
        }
    }

    protected override void OpenPopup()
    {
        _popupService.ShowPopup<RecentArtistPopupViewModel>(onPresenting: vm => vm.SetItem(this, SelectedItem));
    }

    private IArtistViewModel? AddItemToList(Artist artist, DateTime lastAccessed)
    {
        var viewModel = Items.FirstOrDefault(a => a.Model.Id == artist.Id);
        if (viewModel == null)
        {
            var task = _serviceProvider.GetRequiredService<IAddRecentArtistTask>();
            task.Execute(this, artist);
        }
        else
        {
            var task = _serviceProvider.GetRequiredService<IUpdateLastAccessedTask>();
            task.Execute(viewModel.Model);
        }

        return viewModel;
    }

    private void LoadItem(IArtistViewModel item)
    {
        if (item!.LoadStatus == LoadState.NotLoaded)
        {
            item!.LoadStatus = LoadState.Loading;
            var task = _serviceProvider.GetService<IArtistAlbumsLoaderTask>();
            task?.Execute(item);
        }
    }

    private void OnFindItem(FindItemMessage message)
    {
        if (message.PageType == PageType.Artists)
        {
            var viewModel = Items.FirstOrDefault(a => a.Id == message.Id);
            if (viewModel != null)
            {
                viewModel.LastAccessed = DateTime.Now;
                SelectedItem = viewModel;
                return;
            }

            var task = _serviceProvider.GetRequiredService<IFindArtistTask>();
            task.Execute(this, message.Id);
        }
    }

    private void OnSetActiveArtist(ActiveItemChangedMessage message)
    {
        if (message.PageType == PageType.Artists)
        {
            if (message.Model is Artist artist)
            {

                SelectedItem = AddItemToList(artist, DateTime.Now);
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
