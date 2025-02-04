using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Services;

namespace ChasBWare.SpotLight.Infrastructure.Tasks;

public class SetHatedTrackTask(IHatedService _hatedService)
           : ISetHatedTrackTask
{
    public void Execute(ITrackViewModel viewModel)
    {
        viewModel.IsHated = !viewModel.IsHated;
        _hatedService.SetIsHated(viewModel.Id, viewModel.IsHated);
    }
}
