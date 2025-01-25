namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IRecentViewModel<T> : ISortedListViewModel<T> where T : class
    {
        ISearchViewModel<T> SearchViewModel { get; }
        void Initialise();
    }
}
