using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public abstract class BaseRecentViewModel<T> 
                    : BaseSortedListViewModel<T>, 
                      IRecentViewModel<T> 
                      where T: class
{
    private bool _initialised;
    protected readonly IPopupService _popupService;

    public BaseRecentViewModel(IPopupService popupService,
                               IServiceProvider serviceProvider,
                               IPlayerControlViewModel playerControlViewModel,
                               ISearchViewModel<T> searchViewModel,
                               IPropertyComparer<T>[] sorters)
        : base(serviceProvider, sorters)
    {
        _popupService = popupService;
        SearchViewModel = searchViewModel;
        PlayerControlViewModel = playerControlViewModel;
        LoadItems();
        OpenPopupCommand = new Command(track => OpenPopup());
        LoadSettings();
    }

    public ICommand OpenPopupCommand { get; }
    public ISearchViewModel<T> SearchViewModel { get; }
    public IPlayerControlViewModel PlayerControlViewModel { get; }
    public abstract PageType PageType { get; }

    public void Initialise()
    {
        if (_initialised)
        {
            return;
        }

        LoadItems();
        _initialised = true;
    }

    public virtual void OnNavigationRecieved(Uri? callerUri)
    {
    }

    protected abstract void LoadItems();
    protected abstract void OpenPopup();
  
}
