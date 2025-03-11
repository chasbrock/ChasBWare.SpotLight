using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class SearchUserViewModel(IServiceProvider serviceProvider,
                                 IMessageService<ActiveItemChangedMessage> _messageService)
           : BaseSearchViewModel<IUserViewModel>(serviceProvider),
             ISearchUserViewModel
{

    public override void OpenInViewer(IUserViewModel? viewModel)
    {
        _messageService.SendMessage(new ActiveItemChangedMessage(PageType.Users, viewModel?.Model));
    }

    protected override void ExecuteSearch()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            return;
        }

        var searchTask = _serviceProvider.GetRequiredService<ISearchForUserTask>();
        searchTask.Execute(this);
    }
}