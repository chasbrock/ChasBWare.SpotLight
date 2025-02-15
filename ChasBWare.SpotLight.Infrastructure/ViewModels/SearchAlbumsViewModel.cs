using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class SearchAlbumsViewModel(IServiceProvider serviceProvider,
                                      IMessageService<ActiveAlbumChangedMessage> _messageService) 
               : BaseSearchViewModel<IPlaylistViewModel>(serviceProvider), 
                 ISearchAlbumsViewModel
    {
        public override void OpenInViewer(IPlaylistViewModel viewModel)
        {
            _messageService.SendMessage(new ActiveAlbumChangedMessage(viewModel.Model));
        }

        protected override void ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return;
            }

            var searchTask = _serviceProvider.GetService<ISearchForAlbumTask>();
            if (searchTask != null)
            {
                searchTask.Execute(this);
            }
        }
    }
}
