using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public abstract class BaseSearchViewModel<T>(IServiceProvider serviceProvider)
                  : BaseListViewModel<T>(serviceProvider),
                    ISearchViewModel<T> 
                    where T: class
    {
        private bool _isPopupOpen = false;
        private string _searchText = string.Empty;

        public ICommand ExecuteSearchCommand => new Command(()=>ExecuteSearch());

        public ObservableCollection<T> FoundItems { get; } = [];
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
             
        protected override void SelectedItemChanged(T? oldItem, T? newItem)
        {
            if (newItem != null)
            {
                OpenInViewer(newItem);
                IsPopupOpen = false;
            }
        }

        protected override void LoadSettings()
        {
        }

        protected abstract void ExecuteSearch();
        public abstract void OpenInViewer(T viewModel);

        public override void RefreshView()
        {
            FoundItems.Clear();
            foreach (var item in Items)
            {
                FoundItems.Add(item);
            }
        }

    }
}
