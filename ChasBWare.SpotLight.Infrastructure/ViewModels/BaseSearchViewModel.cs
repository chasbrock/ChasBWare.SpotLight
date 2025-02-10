using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public abstract class BaseSearchViewModel<T> : Notifyable, ISearchViewModel<T> where T: class
    {
        private bool _isPopupOpen = false;
        protected readonly IServiceProvider _serviceProvider;
        private T? _selectedItem = default(T);
        private string _searchText = string.Empty;

        protected BaseSearchViewModel(IServiceProvider provisioner)
        {
            _serviceProvider = provisioner;
        }

        public ObservableCollection<T> Items { get; } = [];
        public ICommand ExecuteSearchCommand => new Command(()=>ExecuteSearch());

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                SetField(ref _isPopupOpen, value);
                Notify(nameof(Items));
            }
        }

         public string SearchText
        {
            get => _searchText;
            set => SetField(ref _searchText, value);
        }

        public T? SelectedItem 
        {
            get => _selectedItem;
            set 
            {
                if (SetField(ref _selectedItem, value) && SelectedItem != null)
                {
                    OpenInViewer(SelectedItem);
                    IsPopupOpen = false;
                }
            }
        }

        protected abstract void ExecuteSearch();
        public abstract void OpenInViewer(T viewModel);

    }
}
