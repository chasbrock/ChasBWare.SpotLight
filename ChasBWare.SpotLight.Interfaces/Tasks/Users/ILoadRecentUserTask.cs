using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Users;

public interface ILoadRecentUserTask
{
    void Execute(IRecentUserViewModel viewModel);
}
