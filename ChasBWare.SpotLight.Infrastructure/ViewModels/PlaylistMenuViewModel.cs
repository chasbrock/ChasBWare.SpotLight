using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Popups.ViewModels
{
    public class PlaylistMenuViewModel
               : Notifyable
    {
        private bool _saveToLibrary;
        private bool _love;
        private bool _hate;

        public bool Hate
        {
            get => _hate;
            set => SetField(ref _hate, value);
        }

        public bool Love
        {
            get => _love;
            set => SetField(ref _love, value);
        }

        public bool SaveToLibrary
        {
            get => _saveToLibrary;
            set => SetField(ref _saveToLibrary, value);
        }
    
    }
}
