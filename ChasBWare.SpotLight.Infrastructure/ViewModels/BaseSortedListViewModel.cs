using System.Collections.ObjectModel;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public abstract class BaseSortedListViewModel<T>
                    : BaseListViewModel<T>,
                      ISortedListViewModel<T>
                      where T : class
{
    private IPropertyComparer<T> _selectedSorter;
    private ObservableCollection<T> _sortedItems = [];

    protected BaseSortedListViewModel(IServiceProvider serviceProvider,
                                      IPropertyComparer<T>[] sorters)
            : base(serviceProvider)
    {
        Sorters = sorters;
        _selectedSorter = Sorters[0];
    }

    public IPropertyComparer<T>[] Sorters { get; }

    public List<string> SorterNames
    {
        get => Sorters.Select(g => g.PropertyName).ToList();
    }

    public string SelectedSorterName
    {
        get => SelectedSorter?.PropertyName ?? string.Empty;
        set { SelectedSorter = Sorters.FirstOrDefault(g => g.PropertyName == value) ?? Sorters.First(); }
    }

    public IPropertyComparer<T> SelectedSorter
    {
        get => _selectedSorter;
        set
        {
            if (SetField(ref _selectedSorter, value) &&
                _selectedSorter != null)
            {
                RefreshView();
            }
        }
    }

    public ObservableCollection<T> SortedItems
    {
        get => _sortedItems;
        set => SetField(ref _sortedItems, value);
    }

    public async override void RefreshView()
    {
        if (_selectedSorter == null)
        {
            SortedItems = new ObservableCollection<T>(Items);
        }
        else
        {
            var sorted = Items.ToList();
            sorted.Sort(_selectedSorter);
            SortedItems = new ObservableCollection<T>(sorted);
        }

        var settingsRepo = _serviceProvider.GetService<IAppSettingsRepository>();
        if (settingsRepo != null)
        {
            await settingsRepo.Save(this.BuildKey(nameof(SelectedSorterName)), SelectedSorterName);
        }
    }

    protected override async void LoadSettings()
    {
        var settingsRepo = _serviceProvider.GetService<IAppSettingsRepository>();
        if (settingsRepo != null)
        {
            var found = await settingsRepo.Load(this.BuildKey(nameof(SelectedSorterName)));
            if (found != null)
            {
                SelectedSorterName = found;
            }
        }
    }

    protected override void LoadStatusChanged(LoadState loadStatus)
    {
        RefreshView();
    }
}
