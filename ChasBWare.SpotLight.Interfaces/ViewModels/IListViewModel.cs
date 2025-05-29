using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Definitions.ViewModels;

public interface IListViewModel<T>
               : INotifyable
where T : class
{
    List<T> Items { get; }
    T? SelectedItem { get; set; }
    LoadState LoadStatus { get; set; }

    void RefreshView();
}
