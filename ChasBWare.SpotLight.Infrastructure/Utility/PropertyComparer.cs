using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Utility;
using System.Reflection;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    public class PropertyComparer<T>(string propertyName, SortDirection sortDirection = SortDirection.Ascending)
               : IPropertyComparer<T>
    {
        private readonly SortDirection _sortDirection = sortDirection;
        private readonly PropertyInfo _propertyInfo = typeof(T).GetProperty(propertyName) ??
                                                      throw new MissingFieldException($"Field Name '{propertyName}' is not valid for type {typeof(T).Name}");

        public string PropertyName { get; } = propertyName;

        public int Compare(T? x, T? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x is null)
            {
                return _sortDirection == SortDirection.Ascending ? -1 : 1;
            }
            else if (y is null)
            {
                return _sortDirection == SortDirection.Ascending ? 1 : -1;
            }
            else
            {
                var xx = _propertyInfo.GetValue(x);
                var yy = _propertyInfo.GetValue(y);

                if (ReferenceEquals(x, y))
                {
                    return 0;
                }

                if (xx == null)
                {
                    return _sortDirection == SortDirection.Ascending ? -1 : 1;
                }
                else if (yy == null)
                {
                    return _sortDirection == SortDirection.Ascending ? 1 : -1;
                }

                int result;
                if (_propertyInfo.PropertyType is IComparable)
                {
                    result = ((IComparable)xx).CompareTo(yy);
                }
                else if (xx.IsNumeric())
                {
                    result = decimal.Compare(Convert.ToDecimal(xx), Convert.ToDecimal(yy));
                }
                else if (xx is DateTime)
                {
                    result = DateTime.Compare(Convert.ToDateTime(xx), Convert.ToDateTime(yy));
                }
                else
                {
                    result = string.Compare(xx.ToString(), yy.ToString());
                }
                return _sortDirection == SortDirection.Ascending ? result : result * -1;
            }
        }

        public override string ToString()
        {
            return PropertyName;
        }

    }
}
