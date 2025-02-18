using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class SearchArtistsViewModel(IServiceProvider serviceProvider,
                                    IMessageService<ActiveArtistChangedMessage> _messageService)
           : BaseSearchViewModel<IArtistViewModel>(serviceProvider), 
             ISearchArtistsViewModel
{

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
