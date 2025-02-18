using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui;
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
                                  IMessageService<ActiveArtistChangedMessage> activeArtistChangedMessageService)
         : base(popupService, serviceProvider, playerControlViewModel, searchViewModel, SorterHelper.GetArtistSorters())
    {
        findItemMessageService.Register(OnFindItem);
        activeArtistChangedMessageService.Register(OnSetActiveArtist);
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
        if (message.Payload.PageType == PageType.Artists)
        {
            var viewModel = Items.FirstOrDefault(a => a.Id == message.Payload.Id);
            if (viewModel != null)
            {
                viewModel.LastAccessed = DateTime.Now;
                SelectedItem = viewModel;
                return;
            }

            var task = _serviceProvider.GetRequiredService<IFindArtistTask>();
            task.Execute(this, message.Payload.Id); 
         }
    }      

    private void OnSetActiveArtist(ActiveArtistChangedMessage message)
    {
        SelectedItem = AddItemToList(message.Payload, DateTime.Now);
        if (SelectedItem != null)
        {
            LoadItem(SelectedItem);
        }
        RefreshView();
    }

  
}
