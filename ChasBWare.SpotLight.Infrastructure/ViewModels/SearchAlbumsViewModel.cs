using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class SearchAlbumsViewModel : BaseSearchViewModel<IPlaylistViewModel>, ISearchAlbumsViewModel
    {
        private readonly IMessageService<ActiveAlbumChangedMessage> _messageService;

        public SearchAlbumsViewModel(IServiceProvider serviceProvider,
                                      IMessageService<ActiveAlbumChangedMessage> messageService)
            : base(serviceProvider)
        {
            _messageService = messageService;
        }

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
