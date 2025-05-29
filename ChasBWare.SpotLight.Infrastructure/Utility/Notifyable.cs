using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    public partial class Notifyable : INotifyPropertyChanged, INotifyable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        ///     Sets value of field, and if it has changed a notify event will be fired
        /// trigger the nofication event. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName == null)
            {
                return false;
            }

            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;

                NotifyChanges(propertyName);
                return true;
            }

            return false;
        }


        /// <summary>
        ///     Sets value of field, and if it has changed a notify event will be fired
        ///     use this method where we are simply mapping the underlying model
        /// </summary>
        /// <typeparam name="T">type of data being changed</typeparam>
        /// <param name="dataObject">the member being set</param>
        /// <param name="value">the value to be set</param>
        /// <param name="propertyName">name of property being set</param>
        /// <returns></returns>
        public bool SetField<T>(object dataObject, T value, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName == null || dataObject == null)
            {
                return false;
            }

            PropertyInfo? property = dataObject.GetType().GetProperty(propertyName);
            if (property == null)
            {
                return false;
            }

            T? current = default;
            object? val = property.GetValue(dataObject);
            if (val != null)
            {
                current = (T)val;
            }

            if (!EqualityComparer<T>.Default.Equals(current, value))
            {
                property.SetValue(dataObject, value);
                NotifyChanges(propertyName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// notify all known fields
        /// </summary>
        /// <param name="cascade">if true trigger notification of any notifyable children. 
        /// be carefull not to cause recursive triggering</param>
        public void NotifyAll(bool cascade = false)
        {
            foreach (var property in GetType().GetProperties())
            {
                OnPropertyChanged(property.Name);
                if (cascade && property.GetValue(null) is Notifyable notifyable)
                {
                    notifyable.NotifyAll();
                }
            }
        }

        public void Notify(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        ///     method to call to trigger notification event
        /// </summary>
        /// <param name="propertyName">
        ///     either the name of a property, or null which will default to
        ///     calling member name (ie setting property)
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch { }
        }

        protected void NotifyChanges(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

    }
}
