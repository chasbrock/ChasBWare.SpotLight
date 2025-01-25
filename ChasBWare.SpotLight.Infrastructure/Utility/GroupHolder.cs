using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    public class GroupHolder<TItem>(IGroupedListViewModel<TItem> owner, object key, List<TItem> items) : IGroupHolder<TItem> where TItem : class
    {
        public IGroupedListViewModel<TItem> Owner { get; } = owner;
        public object Key { get; } = key;
        public List<TItem> Items { get; } = items;
        public bool IsExpanded { get; set; }
        public TItem? SelectedItem
        {
            get => Owner.SelectedItem;
            set => Owner.SelectedItem = value;
        }
    }
}
