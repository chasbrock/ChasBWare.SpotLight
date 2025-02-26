using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;

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
