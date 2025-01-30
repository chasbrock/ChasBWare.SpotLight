using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public abstract class BaseListViewModel<T>
                        : Notifyable,
                          IListViewModel<T> where T : class
    {

        protected readonly IServiceProvider _serviceProvider;
        private T? _selectedItem;
        private List<T> _items = [];
        private LoadState _loadStatus = LoadState.NotLoaded;

        protected BaseListViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool IsUpdating{ get; set;}
           

        public List<T> Items
        {
            get => _items;
            set
            {
                if (SetField(ref _items, value))
                {
                    LoadStatusChanged(LoadStatus);
                    if (value.Count > 0)
                    {
                        SelectedItem = value[0];
                    }
                }
            }
        }

        public LoadState LoadStatus
        {
            get => _loadStatus;
            set
            {
                if (SetField(ref _loadStatus, value) &&
                    LoadStatus == LoadState.Loaded)
                {
                    LoadStatusChanged(LoadStatus);
                }
            }
        }


        public T? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetField(ref _selectedItem, value) && _selectedItem != null && !IsUpdating)
                {
                    SelectedItemChanged(_selectedItem);
                }
            }
        }

        protected abstract void LoadSettings();

        protected virtual void LoadStatusChanged(LoadState loadStatus) 
        { 
        }

        protected virtual void SelectedItemChanged(T selectedItem)
        {
        }
    }
}
