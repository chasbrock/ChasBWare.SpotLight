using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{

    public abstract class BaseGroupedListViewModel<T>
                   : BaseListViewModel<T>,
                     IGroupedListViewModel<T> where T : class
    {
        private List<GroupHolder<T>> _groupedItems = [];
        private IGrouper<T> _selectedGrouper;

        protected BaseGroupedListViewModel(IServiceProvider serviceProvider,
                                           IGrouper<T>[] groupers)
                       : base(serviceProvider)
        {
            Groupers = groupers;
            _selectedGrouper = Groupers[0];
        }

     
        public List<GroupHolder<T>> GroupedItems
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

 
        protected void UpdateGroupings()
        {
            GroupedItems = _selectedGrouper.BuildGroups(Items);
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
