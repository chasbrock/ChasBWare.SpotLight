using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Utility;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{

    public class Grouper<T>(string name,
                                 Func<T, object> _getGroupKey,
                                 SortDirection _groupSortDirection,
                                 IPropertyComparer<T> _subListSorter) 
               : IGrouper<T>
    {
        public string Name { get; } = name;

        public List<GroupHolder<T>> BuildGroups(List<T> values)
        {
            // build hashmap with groups
            var keys = new Dictionary<object, List<T>>();
            foreach (var value in values)
            {
                var key = _getGroupKey(value);
                if (keys.TryGetValue(key, out var list))
                {
                    list.Add(value);
                }
                else
                {
                    keys.Add(key, [value]);
                }
            }

            return SortGroups(keys);
        }


        private List<GroupHolder<T>> SortGroups(Dictionary<object, List<T>> groups)
        {
            List<GroupHolder<T>> grouped;
            if (_groupSortDirection == SortDirection.Ascending)
            {
                grouped = groups.Select(nvp => new GroupHolder<T>(nvp.Key, nvp.Value)).OrderBy(gh => gh.Key).ToList();
            }
            else
            {
                grouped = groups.Select(nvp => new GroupHolder<T>(nvp.Key, nvp.Value)).OrderByDescending(gh => gh.Key).ToList();
            }

            foreach (var group in grouped)
            {
                group.Sort(_subListSorter);
            }

            return grouped;
        }

    }


}
