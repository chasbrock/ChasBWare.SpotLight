﻿using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Library;

public class SetHatedTrackTask(IHatedService _hatedService)
           : ISetHatedTrackTask
{
    public void Execute(ITrackViewModel viewModel)
    {
        viewModel.IsHated = !viewModel.IsHated;
        _hatedService.SetIsHated(viewModel.Id, viewModel.IsHated);
    }
}
