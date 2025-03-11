namespace ChasBWare.SpotLight.Definitions.ViewModels;

public enum LibrarySearchTypes 
{ 
    Name,
    Owner
}


public interface ISearchLibraryViewModel
               : ISearchViewModel<IPlaylistViewModel>
{
    List<LibrarySearchTypes> SearchTypes { get; }
    LibrarySearchTypes SelectedSearchType { get; set; }
}
