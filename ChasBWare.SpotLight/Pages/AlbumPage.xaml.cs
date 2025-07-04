using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Pages;

public partial class AlbumPage : ContentPage
{
    public AlbumPage(IRecentAlbumsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        ((IRecentAlbumsViewModel)BindingContext).PlayerControlViewModel.NotifyAll();
    }
}