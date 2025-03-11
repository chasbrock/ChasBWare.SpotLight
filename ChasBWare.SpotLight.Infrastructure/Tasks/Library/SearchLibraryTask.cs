using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Library;

public class SearchLibraryTask(ILibraryViewModel _library )
           : ISearchLibraryTask
{
    public void Execute(ISearchLibraryViewModel viewModel)
    {
        viewModel.OpenInViewer(null);
        viewModel.FoundItems.Clear();

        List<IPlaylistViewModel> found;

        switch (viewModel.SelectedSearchType) 
        {
            case LibrarySearchTypes.Name:
                found = _library.Items.Where(i => i.Name.Contains(viewModel.SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
        
                case LibrarySearchTypes.Owner:
                found = _library.Items.Where(pl => pl.Owners.Any(o => o.Key.Contains(viewModel.SearchText, StringComparison.OrdinalIgnoreCase))).ToList();
                break;

            default: 
                return;
        }


        foreach (var playlist in found)
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

