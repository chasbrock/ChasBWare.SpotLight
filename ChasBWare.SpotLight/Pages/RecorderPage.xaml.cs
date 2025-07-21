using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.ViewModels;

namespace ChasBWare.SpotLight.Pages;

public partial class RecorderPage : ContentPage
{
	public RecorderPage(IRecorderViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
   //     Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
   //     ((IRecentPlaylistsViewModel)BindingContext).PlayerControlViewModel.NotifyAll();
    }

}