using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class SearchAlbumsViewModel(IServiceProvider serviceProvider,
                                      IMessageService<ActiveItemChangedMessage> _messageService) 
               : BaseSearchViewModel<IPlaylistViewModel>(serviceProvider), 
                 ISearchAlbumsViewModel
    {
        public override void OpenInViewer(IPlaylistViewModel? viewModel)
        {
            _messageService.SendMessage(new ActiveItemChangedMessage(PageType.Albums, viewModel?.Model));
        }

        protected override void ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return;
            }

            var searchTask = _serviceProvider.GetRequiredService<ISearchForAlbumTask>();
            searchTask.Execute(this);
        }
    }
}
