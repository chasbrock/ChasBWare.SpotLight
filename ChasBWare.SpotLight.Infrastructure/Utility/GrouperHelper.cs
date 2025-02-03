using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    internal static class GrouperHelper
    {
        public static DateGroups GroupDate(this DateTime value)
        {
            var deltaDays = (DateTime.Today - value.Date).TotalDays;
            return deltaDays switch
            {
                < 1 => DateGroups.Today,
                < 2 => DateGroups.Yesterday,
                < 7 => DateGroups.ThisWeek,
                < 14 => DateGroups.LastWeek,
                < 31 => DateGroups.LastMonth,
                _ => DateGroups.Older
            };
        }

        internal static IGrouper<IPlaylistViewModel>[] GetPlaylistGroupers()
        {
            return [new Grouper<IPlaylistViewModel>(nameof(IPlaylistViewModel.LastAccessed),
                                                   i=> i.LastAccessed.GroupDate(),
                                                   SortDirection.Ascending,
                                                   new PropertyComparer<IPlaylistViewModel>(nameof(IPlaylistViewModel.Name)),
                                                    (k,v) => new PlaylistGroup(k,v)),

                    new Grouper<IPlaylistViewModel>(nameof(IPlaylistViewModel.PlaylistType),
                                                   i=> i.PlaylistType,
                                                   SortDirection.Ascending,
                                                   new PropertyComparer<IPlaylistViewModel>(nameof(IPlaylistViewModel.Name)),
                                                   (k,v) => new PlaylistGroup(k,v)),

                    new Grouper<IPlaylistViewModel>(nameof(IPlaylistViewModel.Owner),
                                                   i=> i.Owner,
                                                   SortDirection.Ascending,
                                                   new PropertyComparer<IPlaylistViewModel>(nameof(IPlaylistViewModel.Name)),
                                                   (k,v) => new PlaylistGroup(k,v))];
        }

    }
}
