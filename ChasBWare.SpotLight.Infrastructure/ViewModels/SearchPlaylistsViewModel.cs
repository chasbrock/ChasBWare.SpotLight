using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class SearchPlaylistsViewModel(IServiceProvider serviceProvider,
                                      IMessageService<ActiveItemChangedMessage> _messageService)
          : BaseSearchViewModel<IPlaylistViewModel>((serviceProvider)),
            ISearchPlaylistsViewModel
{
    public override void OpenInViewer(IPlaylistViewModel? viewModel)
    {
        _messageService.SendMessage(new ActiveItemChangedMessage(PageType.Playlists, viewModel?.Model));
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
