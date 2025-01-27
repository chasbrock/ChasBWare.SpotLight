using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class SearchArtistsViewModel 
               : BaseSearchViewModel<IArtistViewModel>, 
                 ISearchArtistsViewModel
    {
        private readonly IMessageService<ActiveArtistChangedMessage> _messageService;

        public SearchArtistsViewModel(IServiceProvider serviceProvider,
                                      IMessageService<ActiveArtistChangedMessage> messageService)
            : base(serviceProvider)
        {
            _messageService = messageService;
        }

        public override void OpenInViewer(IArtistViewModel viewModel)
        {
            _messageService.SendMessage(new ActiveArtistChangedMessage(viewModel.Model));
        }

        protected override void ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return;
            }

            var searchTask = _serviceProvider.GetService<ISearchForArtistTask>();
            if (searchTask != null)
            {
                searchTask.Execute(this);
            }
        }
    }
}
