using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{

    public class ArtistViewModel(IServiceProvider serviceProvider)
               : PlaylistListViewModel(serviceProvider),
                 IArtistViewModel
    {
        private Artist _model = new() { Id = "" };
        private DateTime _lastPlayed;

        public Artist Model
        {
            get => _model;
            set => SetField(ref _model, value);
        }
             
        public string Id
        {
            get => Model.Id ?? string.Empty;
            set => SetField(Model, value);
        }

        public string? Image
        {
            get => Model.Image;
            set => SetField(Model, value);
        }

        public string Name
        {
            get => Model.Name ?? string.Empty;
            set => SetField(Model, value);
        }

        public DateTime LastAccessed
        {
            get => _lastPlayed;
            set => SetField(ref _lastPlayed, value);
        }
        
        protected override void InitialiseSelectedItem(IPlaylistViewModel selectedItem)
        {
            if (SelectedItem != null &&
                SelectedItem.TracksViewModel.LoadStatus == LoadState.NotLoaded)
            {
                SelectedItem.TracksViewModel.LoadStatus = LoadState.Loading;
                var loadPlaylistTracksTask = _serviceProvider.GetService<IArtistAlbumsLoaderTask>();
                loadPlaylistTracksTask?.Execute(this);
            }
        }

        public override string ToString()
        {
            return Name;
        }
      
    }
}
