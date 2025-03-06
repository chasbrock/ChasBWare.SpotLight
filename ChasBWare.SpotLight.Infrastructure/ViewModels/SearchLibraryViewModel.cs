using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class SearchLibraryViewModel(IServiceProvider serviceProvider,
                                        IMessageService<ActiveItemChangedMessage> _messageService)
              : BaseSearchViewModel<IPlaylistViewModel>(serviceProvider),
                ISearchLibraryViewModel
    {
        public override void OpenInViewer(IPlaylistViewModel item)
        {
            _messageService.SendMessage(new ActiveItemChangedMessage(PageType.Library, item.Model));
        }

        protected override void ExecuteSearch()
        {
            var task = _serviceProvider.GetRequiredService<ISearchLibraryTask>();
            task.Execute(this);
        }
    }
}
