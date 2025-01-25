namespace ChasBWare.SpotLight.Definitions.Utility
{
    public interface IPropertyComparer<T> : IComparer<T>
    {
        public string PropertyName { get;}
    }
}
