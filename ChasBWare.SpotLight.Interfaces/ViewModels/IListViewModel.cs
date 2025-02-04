using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IListViewModel<T> where T : class
    {
        List<T> Items { get; }
        T? SelectedItem { get; set; }
        LoadState LoadStatus { get; set; }

        void RefreshView();
    }
}
