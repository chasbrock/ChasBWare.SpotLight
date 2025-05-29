using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public partial class TrackViewModel
                   : Notifyable, ITrackViewModel
{
    private readonly IMessageService<PlayPlaylistMessage> _messageService;

    private bool _isHated = false;
    private Track _model = new Track { Id = "" };

    public TrackViewModel(IMessageService<PlayPlaylistMessage> messageService)
    {
        _messageService = messageService;
    }

    public ICommand SetHatedTrackCommand { get; set; } = new Command<ITrackViewModel>(o => o.IsHated = !o.IsHated);
    public ICommand PlayTrackCommand { get; } = new Command<ITrackViewModel>(vm => vm.PlayTrackList());

    public IPlaylistViewModel? Playlist { get; set; }

    public Track Model
    {
        get => _model;
        set
        {
            _model = value;
            Artists = _model!.Artists!.UnpackOwners() ?? [];
        }
    }

    public string Album
    {
        get => Model.Album;
    }

    public List<KeyValue> Artists { get; private set; } = [];

    public string Duration
    {
        get => Model.Duration.MSecsToMinsSecs();
    }

    public string Id
    {
        get => Model.Id ?? "";
    }

    public string Name
    {
        get => Model.Name;
    }

    public int TrackNumber
    {
        get => Model.TrackNumber;
    }

    public bool IsHated
    {
        get => _isHated;
        set => SetField(ref _isHated, value);
    }

    public TrackStatus Status
    {
        get => Model.Status;
        set
        {
            SetField(Model, value);
        }
    }

    public void PlayTrackList()
    {
        if (Playlist != null)
        {
            var offset = Playlist.TracksViewModel.Items.ToList().FindIndex(tm => tm.Id == Id);
            _messageService.SendMessage(new PlayPlaylistMessage(Playlist.Model, offset));
        }
    }

    public override string ToString()
    {
        return Model.Name;
    }

}
