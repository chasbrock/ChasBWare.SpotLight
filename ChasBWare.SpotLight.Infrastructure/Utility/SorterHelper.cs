
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Utility;

internal static class SorterHelper
{
    internal static IPropertyComparer<IArtistViewModel>[] GetArtistSorters()
    {
        return [new PropertyComparer<IArtistViewModel>(nameof(IArtistViewModel.LastAccessed), SortDirection.Ascending),
                new PropertyComparer<IArtistViewModel>(nameof(IArtistViewModel.Name))];
    }

    internal static IPropertyComparer<IPlaylistViewModel>[] GetPlaylistSorters()
    {
        return [  new PropertyComparer<IPlaylistViewModel>(nameof(IPlaylistViewModel.LastAccessed)),
                  new PropertyComparer<IPlaylistViewModel>(nameof(IPlaylistViewModel.Name)),
                  new PropertyComparer<IPlaylistViewModel>(nameof(IPlaylistViewModel.Owner))];
    }

    internal static IPropertyComparer<IPlaylistViewModel>[] GetPlaylistListSorters()
    {
        return [  new PropertyComparer<IPlaylistViewModel>(nameof(IPlaylistViewModel.ReleaseDate)),
                  new PropertyComparer<IPlaylistViewModel>(nameof(IPlaylistViewModel.Name)),
                  new PropertyComparer<IPlaylistViewModel>(nameof(IPlaylistViewModel.LastAccessed))];
    }

    internal static IPropertyComparer<IUserViewModel>[] GetUserSorters()
    {
        return [new PropertyComparer<IUserViewModel>(nameof(IUserViewModel.LastAccessed), SortDirection.Ascending),
                new PropertyComparer<IUserViewModel>(nameof(IUserViewModel.Name))];
    }
}
