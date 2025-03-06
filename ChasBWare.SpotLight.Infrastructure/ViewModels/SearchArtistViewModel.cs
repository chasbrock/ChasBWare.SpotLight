using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class SearchArtistsViewModel(IServiceProvider serviceProvider,
                                    IMessageService<ActiveItemChangedMessage> _messageService)
           : BaseSearchViewModel<IArtistViewModel>(serviceProvider), 
             ISearchArtistsViewModel
{

    public override void OpenInViewer(IArtistViewModel viewModel)
    {
        _messageService.SendMessage(new ActiveItemChangedMessage(PageType.Artists, viewModel.Model));
    }

    protected override void ExecuteSearch()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            return;
        }

        var searchTask = _serviceProvider.GetRequiredService<ISearchForArtistTask>();
        searchTask.Execute(this);
    }
}
