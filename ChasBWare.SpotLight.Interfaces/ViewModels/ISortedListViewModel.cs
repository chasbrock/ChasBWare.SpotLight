using System.Collections.ObjectModel;

using ChasBWare.SpotLight.Definitions.Utility;

namespace ChasBWare.SpotLight.Definitions.ViewModels;

public interface ISortedListViewModel<T> 
               : IListViewModel<T> 
               where T: class
{
    ObservableCollection<T> SortedItems { get; }
    IPropertyComparer<T>[] Sorters { get; }
    IPropertyComparer<T> SelectedSorter { get; set; }
    List<string> SorterNames { get; }
    string SelectedSorterName { get; set; }
}
