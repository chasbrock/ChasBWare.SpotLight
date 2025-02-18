using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface ISearchViewModel<T>
                   : IListViewModel<T> 
                     where T: class
    {
        public ICommand ExecuteSearchCommand { get; }
        string SearchText { get; set; }
        bool IsPopupOpen { get; set; }

        ObservableCollection<T> FoundItems { get; }

        void OpenInViewer(T viewModel);
    }
}
