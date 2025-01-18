using System;
using System.Collections.Generic;
using System.Linq;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    
    public interface ISearchArtistsViewModel : ISearchViewModel<IArtistViewModel>
    {
    }

    public interface ISearchAlbumsViewModel : ISearchViewModel<IPlaylistViewModel>
    {
    }

    public interface ISearchPlaylistsViewModel : ISearchViewModel<IPlaylistViewModel>
    {
    }
}
