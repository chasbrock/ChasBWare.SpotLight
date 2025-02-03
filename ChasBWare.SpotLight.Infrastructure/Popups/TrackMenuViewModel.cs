using System.ComponentModel;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.Popups
{
    public partial class TrackMenuViewModel
                       : PopupMenuViewModel
    {
        private const string QueueGroup = "Queue";
        private const string HatedGroup = "Hated";

        private readonly IServiceProvider _serviceProvider;
        private readonly IHatedService _hatedService;
        private readonly MenuItem _hateMenuItem;
        private readonly MenuItem _unhateMenuItem;
        private ITrackViewModel? _track = null;
        private bool _isHated;
   
        public TrackMenuViewModel(IPopupService popupService,
                                  IServiceProvider serviceProvider,
                                  IHatedService hatedService)
              : base(popupService)
        {
            _serviceProvider = serviceProvider;
            _hatedService = hatedService;

            AddItem(QueueGroup, "AddToQueue", new Command(DoAddToQueue), "Add Track to queue");
            _hateMenuItem = AddItem(HatedGroup, "Hate", new Command(DoChangeHatedStatus));
            _unhateMenuItem = AddItem(HatedGroup, "Unhate", new Command(DoChangeHatedStatus));
            AddItem("System", "Close", new Command(Close));
        }

        public void SetTrack(ITrackViewModel track)
        {
            _track = track;
            IsHated = _hatedService.GetIsHated(track.Id);
        }

        public bool IsHated
        {
            get => _isHated;
            set
            {
                SetField(ref _isHated, value);
                _hateMenuItem.Visible = IsHated;
                _unhateMenuItem.Visible = !IsHated;
            }
        }

        private void DoChangeHatedStatus()
        {
            IsHated = !IsHated;
            if (_track != null)
            {
                _hatedService.SetIsHated(_track.Id, IsHated);
                _track.IsHated = IsHated;
            }
        }

        private void DoAddToQueue()
        {
            if (_track != null)
            {
                var trackPlayerService = _serviceProvider.GetRequiredService<ITrackPlayerService>();
                trackPlayerService.AddTrackToQueue(_track.Id);
            }
         }
    }
}
