using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public abstract class BaseGroupedListViewModel<T>
                    : BaseListViewModel<T>,
                      IGroupedListViewModel<T> where T : class
{

    private List<IGroupHolder<T>> _groupedItems = [];
    private IGrouper<T> _selectedGrouper;

    protected BaseGroupedListViewModel(IServiceProvider serviceProvider,
                                       IGrouper<T>[] groupers)
                   : base(serviceProvider)
    {
        Groupers = groupers;
        _selectedGrouper = Groupers[0];
        ItemSelectedCommand = new Command(t => SelectedItem = (T)t);
    }

    public void ExpandAll()
    {
        foreach (var group in GroupedItems)
        {
            group.IsExpanded = true;
        }
    }

    public void CollapseAll()
    {
        foreach (var group in GroupedItems)
        {
            group.IsExpanded = false;
        }
    }

    public ICommand ItemSelectedCommand { get; private set; }

    public List<string> GrouperNames
    {
        get => Groupers.Select(g => g.Name).ToList();
    }

    public string SelectedGrouperName
    {
        get => SelectedGrouper?.Name ?? string.Empty;
        set
        {
            SelectedGrouper = Groupers.FirstOrDefault(g => g.Name == value) ?? Groupers.First();
            Notify(nameof(SelectedGrouperName));
        }
    }

    public List<IGroupHolder<T>> GroupedItems
    {
        get => _groupedItems;
        set => SetField(ref _groupedItems, value);
    }


    public IGrouper<T>[] Groupers { get; }

    public IGrouper<T> SelectedGrouper
    {
        get => _selectedGrouper;
        set
        {
            if (SetField(ref _selectedGrouper, value) &&
                _selectedGrouper != null)
            {
                RefreshView();
            }
        }
    }

    protected override async void LoadSettings()
    {
        var settingsRepo = _serviceProvider.GetService<IAppSettingsRepository>();
        if (settingsRepo != null)
        {
            var found = await settingsRepo.Load(this.BuildKey(nameof(SelectedGrouperName)));
            if (found != null)
            {
                SelectedGrouperName = found;
            }

            RefreshView();
        }
    }

    public override void RefreshView()
    {
        GroupedItems = _selectedGrouper.BuildGroups(Items);
        var settingsRepo = _serviceProvider.GetService<IAppSettingsRepository>();
        if (settingsRepo != null)
        {
            Task.Run(() => settingsRepo.Save(this.BuildKey(nameof(SelectedGrouperName)), SelectedGrouperName));
        }
    }

    protected override void LoadStatusChanged(LoadState loadStatus)
    {
        LoadSettings();
    }

    protected override void SelectedItemChanged(T? oldItem, T? newItem)
    {
        if (IsUpdating)
        {
            return;
        }
        if (newItem != null)
        {
            var selectedGroup = GroupedItems.FirstOrDefault(g => g.Items.Contains(newItem));
            if (selectedGroup != null)
            {
                selectedGroup.IsExpanded = true;
            }
        }
    }

}
