using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public abstract class BaseRecentViewModel<T> 
                    : BaseSortedListViewModel<T>, 
                      IRecentViewModel<T> 
                      where T: class
{
    private bool _initialised;

    public BaseRecentViewModel(IServiceProvider serviceProvider,
                               ISearchViewModel<T> searchViewModel,
                               IPropertyComparer<T>[] sorters)
        : base(serviceProvider, sorters)

    {
        SearchViewModel = searchViewModel;
        LoadRecentItems();
    }

    public ICommand DeleteRecentCommand { get; } = new Command<BaseRecentViewModel<T>>(vm => vm?.DeleteItem(),
                                                                                       vm => vm!.SelectedItem != null);
    public ISearchViewModel<T> SearchViewModel { get; }
  
    public void Initialise()
    {
        if (_initialised)
        {
            return;
        }

        LoadRecentItems();
        _initialised = true;
    }

    protected abstract void LoadRecentItems();
    protected abstract void DeleteItem();

}
