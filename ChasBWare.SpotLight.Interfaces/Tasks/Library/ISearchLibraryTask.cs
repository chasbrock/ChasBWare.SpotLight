﻿using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Definitions.Tasks.Library;

public interface ISearchLibraryTask
{
    void Execute(ISearchLibraryViewModel viewModel);
}
