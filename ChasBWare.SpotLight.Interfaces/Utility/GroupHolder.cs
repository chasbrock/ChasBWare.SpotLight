namespace ChasBWare.SpotLight.Definitions.Utility
{
    public class GroupHolder<TValue> : List<TValue>
    {
        public GroupHolder(object key) => Key = key;
        public GroupHolder(object key, IEnumerable<TValue> values) : base(values) => Key = key;

        public object Key { get; }
    }
}
