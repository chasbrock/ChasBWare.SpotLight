using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Users;

public interface ISearchForUserTask
{
    void Execute(ISearchUserViewModel viewModel);
}
