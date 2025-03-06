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
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class PlaylistViewModel : Notifyable, IPlaylistViewModel
{
    private readonly IServiceProvider _serviceProvider;
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
        OpenTrackPopupCommand = new Command<ITrackViewModel>(t => popupService.ShowPopup<TrackPopupViewModel>(onPresenting: vm => vm.SetTrack(this, TracksViewModel.SelectedItem)));
        OpenArtistCommand = new Command<string>(id => NavigateToArtist(id));
    }

    public Playlist Model 
    { 
        get => _model;
        set 
        {
            _model = value;
            Owners = Model!.Owner!.UnpackOwners()??[];
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
        get => Model.Description ?? "";
        set => SetField(Model, value);
    }

    public string Id
    {
        get => Model.Id ?? "";
    }

    public string? Image
    {
        get => Model.Image;
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
        _messageService.SendMessage(new PlayPlaylistMessage(this.Model, 0));
    }

    private void LoadTracks()
    {
        if (TracksViewModel.LoadStatus == LoadState.NotLoaded)
        {
            TracksViewModel.LoadStatus = LoadState.Loading;
            var task = _serviceProvider.GetService<ITrackListLoaderTask>();
            task?.Execute(this);
        }
    }
      
    public string Name
    {
        get => Model.Name??"";
    }

    public List<KeyValue> Owners { get; private set; } = [];
    
    public string Owner
    {
        get => Owners.FirstOrDefault()!.Key ?? string.Empty;
    }

    public PlaylistType PlaylistType
    {
        get => Model.PlaylistType;
    }

    public DateTime ReleaseDate
    {
        get => Model.ReleaseDate;
    }

    public string Uri
    {
        get => Model.Uri ?? string.Empty;
    }

    public DateTime LastAccessed
    {
        get => Model.LastAccessed;
        set => SetField(Model, value); 
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
        messageService.SendMessage(new FindItemMessage(PageType.Artists, id));

        _navigator.NavigateTo(PageType.Artists);
    }

    public override string ToString()
    {
        return Name;
    }
}
