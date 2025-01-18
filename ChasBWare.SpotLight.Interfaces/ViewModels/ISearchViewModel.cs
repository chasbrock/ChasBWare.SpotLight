using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface ISearchViewModel<T> where T : class
    {
        public ICommand ExecuteSearchCommand { get; }
        string SearchText { get; set; }
        bool IsPopupOpen { get; set; }
        ObservableCollection<T> Items { get; }

        void Initialise();
        void OpenInViewer(T viewModel);
    }
}
