namespace ChasBWare.SpotLight.Definitions.Utility
{
    public interface IGrouper<TValue>
    {
        string Name { get; }

        List<GroupHolder<TValue>> BuildGroups(List<TValue> values);
    }
}