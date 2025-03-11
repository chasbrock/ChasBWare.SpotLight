using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Tasks.Users;

public interface IAddRecentUserTask
{
    void Execute(IRecentUserViewModel viewModel, User model);
}
