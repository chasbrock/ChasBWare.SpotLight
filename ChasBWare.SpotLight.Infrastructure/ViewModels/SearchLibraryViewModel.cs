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
          private LibrarySearchTypes _selectedSearchType = LibrarySearchTypes.Name;


        public override void OpenInViewer(IPlaylistViewModel? item)
        {
            _messageService.SendMessage(new ActiveItemChangedMessage(PageType.Library, item?.Model));
        }

        public List<LibrarySearchTypes> SearchTypes { get; } = Enum.GetValues<LibrarySearchTypes>().ToList();

        public LibrarySearchTypes SelectedSearchType
        {
            get => _selectedSearchType;
            set => SetField(ref _selectedSearchType, value);
        }

        protected override void ExecuteSearch()
        {
            var task = _serviceProvider.GetRequiredService<ISearchLibraryTask>();
            task.Execute(this);
        }
    }
}
