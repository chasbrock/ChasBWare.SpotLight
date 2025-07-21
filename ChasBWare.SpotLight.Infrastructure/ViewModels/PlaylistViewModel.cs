using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class PlaylistViewModel : Notifyable, IPlaylistViewModel
{
    protected readonly IServiceProvider _serviceProvider;
    private readonly INavigator _navigator;
    private readonly IMessageService<PlayPlaylistMessage> _messageService;

    private bool _isExpanded = false;
    private bool _isSelected = false;
    private bool _inLibrary = false;
    private Playlist _model = new() { Id = "" };

    public PlaylistViewModel(IServiceProvider serviceProvider,
                             INavigator navigator,
                             ITrackListViewModel tracksViewModel,
                             IPopupService popupService,
                             IMessageService<PlayPlaylistMessage> messageService)
    {
        TracksViewModel = tracksViewModel;
        _serviceProvider = serviceProvider;
        _messageService = messageService;
        _navigator = navigator;

        SetExpandedCommand = new Command(() => IsExpanded = !IsExpanded);
        PlayTracklistCommand = new Command(PlayTrackList);
        OpenTrackPopupCommand = new Command<ITrackViewModel>(t => popupService.ShowPopup<TrackPopupViewModel>(Shell.Current, null, TrackPopupViewModel.BuildParams(this, TracksViewModel.SelectedItem)));
        OpenArtistCommand = new Command<string>(id => NavigateToArtist(id));
    }

    public Playlist Playlist
    {
        get => _model;
        set
        {
            _model = value;
            Owners = Playlist!.Owner!.UnpackOwners() ?? [];
            TracksViewModel.Playlist = this;
        }
    }

    public ICommand OpenTrackPopupCommand { get; }
    public ICommand SetExpandedCommand { get; }
    public ICommand PlayTracklistCommand { get; }
    public ICommand OpenArtistCommand { get; }

    public ITrackListViewModel TracksViewModel { get; }

    public string Description
    {
        get => Playlist.Description ?? "";
        set => SetField(Playlist, value);
    }

    public string Id
    {
        get => Playlist.Id ?? "";
    }

    public string? Image
    {
        get => Playlist.Image;
    }

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (SetField(ref _isExpanded, value) &&
                _isExpanded &&
                TracksViewModel.LoadStatus == LoadState.NotLoaded)
            {
                LoadTracks();
            }
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetField(ref _isSelected, value);
    }

    private void PlayTrackList()
    {
        IsExpanded = true;
        _messageService.SendMessage(new PlayPlaylistMessage(this.Playlist, 0));
    }

    private void LoadTracks()
    {
        if (TracksViewModel.LoadStatus == LoadState.NotLoaded)
        {
            TracksViewModel.LoadStatus = LoadState.Loading;
            var task = _serviceProvider.GetRequiredService<ITrackListLoaderTask>();
            task.Execute(this);
        }
    }

    public string Name
    {
        get => Playlist.Name ?? "";
    }

    public List<KeyValue> Owners { get; private set; } = [];

    public KeyValue? Owner
    {
        get => Owners.FirstOrDefault();
    }

    public PlaylistType PlaylistType
    {
        get => Playlist.PlaylistType;
    }

    public DateTime ReleaseDate
    {
        get => Playlist.ReleaseDate;
    }

    public string Uri
    {
        get => Playlist.Uri ?? string.Empty;
    }

    public DateTime LastAccessed
    {
        get => Playlist.LastAccessed;
        set => SetField(Playlist, value);
    }

    public bool InLibrary
    {
        get => _inLibrary;
        set => SetField(ref _inLibrary, value);
    }

    private void NavigateToArtist(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return;
        }

        var messageService = _serviceProvider.GetRequiredService<IMessageService<FindItemMessage>>();
        if (PlaylistType == PlaylistType.Album)
        {
            messageService.SendMessage(new FindItemMessage(PageType.Artists, id));
            _navigator.NavigateTo(PageType.Artists);
        }
        else
        {
            messageService.SendMessage(new FindItemMessage(PageType.Users, id));
            _navigator.NavigateTo(PageType.Users);
        }
    }


    public override string ToString()
    {
        return Name;
    }
}
