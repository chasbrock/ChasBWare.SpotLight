using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Library;

public class SearchLibraryTask(ILibraryViewModel _library )
           : ISearchLibraryTask
{
    public void Execute(ISearchLibraryViewModel viewModel)
    {
        viewModel.FoundItems.Clear();
        foreach (var playlist in _library.Items.Where(i => i.Name.Contains(viewModel.SearchText, StringComparison.OrdinalIgnoreCase)))
        {
            viewModel.FoundItems.Add(playlist);
        }

        if (viewModel.FoundItems.Count == 0)
        {
            viewModel.IsPopupOpen = false;
        }
        else if (viewModel.FoundItems.Count == 1)
        {
            viewModel.OpenInViewer(viewModel.FoundItems[0]);
            viewModel.IsPopupOpen = false;
        }
        else
        {
            viewModel.IsPopupOpen = true;
        }
    }
}

