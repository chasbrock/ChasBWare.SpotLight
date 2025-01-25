using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{

    public abstract class BaseGroupedListViewModel<T>
                        : BaseListViewModel<T>,
                          IGroupedListViewModel<T> where T : class
    {
        private List<IGroupHolder<T>> _groupedItems = [];
        private IGrouper<T> _selectedGrouper;

        protected BaseGroupedListViewModel(IServiceProvider serviceProvider,
                                           IGrouper<T>[] groupers)
                       : base(serviceProvider)
        {
            Groupers = groupers;
            _selectedGrouper = Groupers[0];
        }

        public List<string> GrouperNames
        {
            get => Groupers.Select(g => g.Name).ToList();
        }
        
        public string SelectedGrouperName
        {
            get => SelectedGrouper?.Name ?? string.Empty;
            set { SelectedGrouper = Groupers.FirstOrDefault(g => g.Name == value) ?? Groupers.First(); }
        }

        public List<IGroupHolder<T>> GroupedItems
        {
            get => _groupedItems;
            set => SetField(ref _groupedItems, value);
        }

   
        public IGrouper<T>[] Groupers { get; }

        public IGrouper<T> SelectedGrouper
        {
            get => _selectedGrouper;
            set
            {
                if (SetField(ref _selectedGrouper, value) &&
                    _selectedGrouper != null)
                {
                    UpdateGroupings();
                }
            }
        }

         protected virtual void UpdateGroupings()
        {
            GroupedItems = _selectedGrouper.BuildGroups(this, Items);
        }

        protected override void LoadStatusChanged(LoadState loadStatus)
        {
            UpdateGroupings();
        }

        protected override void SelectedItemChanged(T? selectedItem)
        {
            if (selectedItem != null)
            {
                InitialiseSelectedItem(selectedItem);
            }
        }

        protected abstract void InitialiseSelectedItem(T selectedItem);

    }
}
