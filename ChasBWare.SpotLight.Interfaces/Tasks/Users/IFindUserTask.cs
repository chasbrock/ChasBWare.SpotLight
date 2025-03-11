using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Users;

public interface IFindUserTask
{
    void Execute(IRecentUserViewModel viewModel, string userId);
}
