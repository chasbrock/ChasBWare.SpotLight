using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class UserViewModel(IServiceProvider serviceProvider)
           : PlaylistListViewModel(serviceProvider),
             IUserViewModel
{
    private User _model = new() { Id = "" };

    public User Model
    {
        get => _model;
        set => SetField(ref _model, value);
    }

    public string Id
    {
        get => Model.Id ?? string.Empty;
    }

    public string? Image
    {
        get => Model.Image;
    }

    public string Name
    {
        get => Model.Name ?? string.Empty;
    }

    public DateTime LastAccessed
    {
        get => Model.LastAccessed;
        set => SetField(Model, value);
    }

    protected override void SelectedItemChanged(IPlaylistViewModel? oldItem, IPlaylistViewModel? newItem)
    {
        if (oldItem != null)
        {
            oldItem.IsSelected = false;
        }

        if (newItem != null)
        {
            newItem.IsSelected = true;
            newItem.IsExpanded = true;

            if (newItem.TracksViewModel.LoadStatus == LoadState.NotLoaded)
            {
                newItem.TracksViewModel.LoadStatus = LoadState.Loading;
                var loadPlaylistTracksTask = _serviceProvider.GetService<IUserAlbumsLoaderTask>();
                loadPlaylistTracksTask?.Execute(this);
            }
        }
    }
}

