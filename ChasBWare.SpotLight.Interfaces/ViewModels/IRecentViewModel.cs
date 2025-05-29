using ChasBWare.SpotLight.Definitions.Utility;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IRecentViewModel<T>
                   : ISortedListViewModel<T>,
                     INavigationClient

                     where T : class
    {
        ISearchViewModel<T> SearchViewModel { get; }
        IPlayerControlViewModel PlayerControlViewModel { get; }

        void Initialise();
    }
}
