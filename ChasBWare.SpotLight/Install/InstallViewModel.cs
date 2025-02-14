using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows.Input;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace ChasBWare.SpotLight.Install;

public partial class InstallViewModel : Notifyable
{
    public const string Initialised = "Initialised";

    private readonly IAlertService _alertService;
    private readonly IPopupService _popupService;

    private string _key = "";

    public InstallViewModel(IAlertService alertService,
                            IPopupService popupService)
    {
        _alertService = alertService;
        _popupService = popupService;

        OkCommand = new Command<Popup>(DoOkCommand);
        CancelCommand = new Command(() => Application.Current!.Quit());
    }

    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public string Key
    {
        get => _key;
        set => SetField(ref _key, value);
    }

    private async void DoOkCommand(object obj)
    {
        try
        {
            var fileStream = await FileSystem.Current.OpenAppPackageFileAsync("SpotLight.vault");
            using StreamReader reader = new StreamReader(fileStream);
            var json = await reader.ReadToEndAsync();
            var items = JsonSerializer.Deserialize<List<KeyValue>>(json);

            if (items == null)
            {
                throw new SystemException("No initialisation data found");
            }

            RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(Key), out _);
            foreach (var item in items)
            {
                var raw = Convert.FromBase64String(item.Value);
                var value = Convert.ToBase64String(rsa.Decrypt(raw, RSAEncryptionPadding.Pkcs1));
                await SecureStorage.Default.SetAsync(item.Key, value);
            }

            await SecureStorage.Default.SetAsync(Initialised, true.ToString());
            
            _popupService.ClosePopup(this);
        }
        catch (CryptographicException crex)
        {
            await _alertService.ShowErrorAsync("Error initialising app", crex.Message, "Close");
            Application.Current!.Quit();
        }
        catch (Exception ex)
        {
            await _alertService.ShowErrorAsync("Error initialising app", ex.Message, "Close");
            Application.Current!.Quit();
        }
    }

}


