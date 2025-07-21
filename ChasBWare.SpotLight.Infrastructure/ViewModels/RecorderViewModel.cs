using System.Reflection;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.Tasks.Recorder;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Domain.Services;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class RecorderViewModel : PlaylistViewModel, IRecorderViewModel
{
    private bool _isRecording = false;
   
    public RecorderViewModel(IServiceProvider serviceProvider,
                             INavigator navigator,
                             ITrackListViewModel tracksViewModel,
                             IPopupService popupService,
                             IPlayerControlViewModel playerControlViewModel,
                             IMessageService<PlayPlaylistMessage> messageService,
                             IMessageService<CurrentTrackChangedMessage> currentTrackChangedMessage) 
                       : base(serviceProvider, navigator, tracksViewModel, popupService, messageService)
    {
        Playlist  = new Playlist { Id = "", Description = "New playlist", Name = "New playlist" };
        PlayerControlViewModel = playerControlViewModel;
        currentTrackChangedMessage.Register(OnTrackChangedMessage);


        ClearListCommand = new Command(c => ClearList());
        DeleteCommand = new Command(c => DeleteTrack());
        MoveDownCommand = new Command(c => MoveTrackDown());
        MoveUpCommand = new Command(c => MoveTrackUp());
        PasteFromClipboardCommand = new Command(c => ReadClipboard());
        UploadCommand = new Command(c => UploadPlaylist());

    }

    public IPlayerControlViewModel PlayerControlViewModel { get; }

    public bool IsRecording
    {
        get => _isRecording;
        set => SetField(ref _isRecording, value);
    }

    public ICommand ClearListCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    public ICommand MoveUpCommand { get; private set; }
    public ICommand MoveDownCommand { get; private set; }
    public ICommand PasteFromClipboardCommand { get; private set; }
    public ICommand UploadCommand { get; private set; }


    public void AddTracks(List<Track> list)
    {
        foreach (var track in list) 
        {
            track.TrackNumber = TracksViewModel.Items.Count();
            var trackViewModel = _serviceProvider.GetRequiredService<ITrackViewModel>();
            trackViewModel.Playlist = this;
            trackViewModel.Model = track;
            TracksViewModel.Items.Add(trackViewModel); 
        }
    }

    private void ClearList()
    {
        TracksViewModel.Items.Clear();
    }

    private void DeleteTrack()
    {
        TracksViewModel.DeleteSelectedItem();
    }

    private void MoveTrackDown()
    {
        TracksViewModel.MoveSelectedTrackDown();
    }

    private void MoveTrackUp()
    {
        TracksViewModel.MoveSelectedTrackUp();
    }

    private void UploadPlaylist()
    {
        throw new NotImplementedException();
    }

    private void ReadClipboard()
    {
        var task = _serviceProvider.GetRequiredService<IParseClipboardTask>();
        task.Execute(this);
    }

    private void OnTrackChangedMessage(CurrentTrackChangedMessage message)
    {
        if (IsRecording && message.State == TrackStatus.Playing && 
            !TracksViewModel.Items.Any(t=>t.Id== message.Track.Id))
        {
            var track = new Track
            {
                Id = message.Track.Id,
                Album = message.Track.Album,
                Duration = (int)message.Track.Duration.TotalMilliseconds,
                Artists = string.Join(';', message.Track.Artists.Select(a=>a.Key)),
                Name = message.Track.Name,
                TrackNumber = TracksViewModel.Items.Count(),
                Uri = message.Track.Uri 
             };

            var trackViewModel = _serviceProvider.GetRequiredService<ITrackViewModel>();
            trackViewModel.Playlist = this;
            trackViewModel.Model = track;
            TracksViewModel.Items.Add(trackViewModel);
        }
    }
}