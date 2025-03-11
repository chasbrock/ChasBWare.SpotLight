using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Users;

public interface IRemoveRecentUserTask
{
    void Execute(IRecentUserViewModel viewModel, IUserViewModel item);
    void Execute(IRecentUserViewModel viewModel);
}
