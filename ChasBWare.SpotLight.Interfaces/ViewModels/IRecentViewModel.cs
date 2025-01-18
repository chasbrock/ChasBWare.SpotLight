namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IRecentViewModel<T> : IListViewModel<T> where T : class
    {
        ISearchViewModel<T> SearchViewModel { get; }
        void Initialise();
    }
}
