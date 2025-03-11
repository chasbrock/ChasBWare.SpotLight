using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Users;

public interface IUserAlbumsLoaderTask
{
    void Execute(IUserViewModel viewModel);
}


