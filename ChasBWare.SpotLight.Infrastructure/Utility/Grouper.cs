using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    /// <summary>
    /// builds groups of items of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">name displayed on picker </param>
    /// <param name="_getGroupKey">function to generate key</param>
    /// <param name="_groupSortDirection">sort direction obvs</param>
    /// <param name="_subListSorter">a comparer to sort the rows in group</param>
    /// <param name="_createHolder">function to create the row holder</param>
    public class Grouper<T>(string name,
                            Func<T, object> _getGroupKey,
                            SortDirection _groupSortDirection,
                            IPropertyComparer<T> _subListSorter,
                            Func<IGroupedListViewModel<T>, object, List<T>, IGroupHolder<T>> _createHolder)
                : IGrouper<T>
                 where T : class
    {
        public string Name { get; } = name;

        public List<IGroupHolder<T>> BuildGroups(IGroupedListViewModel<T> owner, List<T> values)
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

            return SortGroups(owner, keys);
        }

        private List<IGroupHolder<T>> SortGroups(IGroupedListViewModel<T> owner, Dictionary<object, List<T>> groups)
        {
            List<IGroupHolder<T>> grouped;
            if (_groupSortDirection == SortDirection.Ascending)
            {
               grouped = groups.Select(nvp => _createHolder(owner, nvp.Key, nvp.Value)).OrderBy(gh=>gh.Key).ToList(); 
            }
            else
            {
                grouped = groups.Select(nvp => _createHolder(owner, nvp.Key, nvp.Value)).OrderByDescending(gh => gh.Key).ToList();
            }

            foreach (var group in grouped)
            {
                group.Items.Sort(_subListSorter);
            }

            return grouped;
        }
    }
}
