namespace ChasBWare.SpotLight.Definitions.Utility
{
    public interface IGrouper<T> where T : class
    {
        string Name { get; }

        List<IGroupHolder<T>> BuildGroups(List<T> values);
    }
}