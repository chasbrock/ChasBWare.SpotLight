using System.Runtime.CompilerServices;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    public interface INotifyable
    {
        void Notify(params string[] propertyNames);
        void NotifyAll(bool cascade = false);
        bool SetField<T>(object dataObject, T value, [CallerMemberName] string? propertyName = null);
        bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null);
    }
}
