namespace ChasBWare.SpotLight.Definitions.Utility
{
    public interface IGroupHolder<T> where T : class
    {
        bool IsExpanded { get; set; }
        List<T> Items { get; }
        object Key { get; }
    }
}