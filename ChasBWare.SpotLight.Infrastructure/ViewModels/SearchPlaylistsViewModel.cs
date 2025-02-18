using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class SearchPlaylistsViewModel(IServiceProvider serviceProvider,
                                       IMessageService<ActivePlaylistChangedMessage> _messageService)
          : BaseSearchViewModel<IPlaylistViewModel>((serviceProvider)),
            ISearchPlaylistsViewModel
{
    public override void OpenInViewer(IPlaylistViewModel viewModel)
    {
        _messageService.SendMessage(new ActivePlaylistChangedMessage(viewModel.Model));
    }

    protected override void ExecuteSearch()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            return;
        }

        var searchTask = _serviceProvider.GetRequiredService<ISearchForPlaylistTask>();
        searchTask.Execute(this);
    }
}
