using ChasBWare.SpotLight.Definitions.Utility;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IGroupedListViewModel<T> 
                   : IListViewModel<T> where T : class
    {
        List<GroupHolder<T>> GroupedItems { get; }
        IGrouper<T>[] Groupers { get; }
        IGrouper<T> SelectedGrouper { get; set; }
    }


}
