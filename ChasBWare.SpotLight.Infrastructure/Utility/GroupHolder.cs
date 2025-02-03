using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    public class GroupHolder<TItem> : Notifyable, IGroupHolder<TItem> where TItem : class
    {
        private bool _isExpanded;

        public GroupHolder(object key, List<TItem> items)
        {
            Items = items;
            Key = key;
            SetExpandedCommand =  new Command(() => IsExpanded = !IsExpanded); 
        }

        public object Key { get; } 
        public List<TItem> Items { get; }
        public ICommand SetExpandedCommand { get; }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetField(ref _isExpanded, value);
        }
    }
}
