using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Models;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public partial class CurrentDeviceViewModel : Notifyable, ICurrentDeviceViewModel
{
    private readonly IDeviceService _deviceService;
    private DeviceModel _device = DeviceHelper.GetLocalDevice();
    private bool _isMuted = false;
    private int _unmutedVolume = 0;

    public CurrentDeviceViewModel(IDeviceService deviceService)
    {
        _deviceService = deviceService;
        _device = DeviceHelper.GetLocalDevice();
        MuteCommand = new Command(DoMuteChanged);
        VolumeUpdatedCommand = new Command(DoVolumeUpdated);
    }

    public string Name { get => Device.Name; }
    public DeviceTypes DeviceType { get => Device.DeviceType; }
    public ICommand MuteCommand { get; }
    public ICommand VolumeUpdatedCommand { get; }

    public DeviceModel Device
    {
        get => _device;
        set
        {
            _device = value;
            NotifyAll();
        }
    }

    public int VolumePercent
    {
        get => Device.VolumePercent;
        set => SetField(Device, value);
    }

    public bool IsActive
    {
        get => Device.IsActive;
        set => SetField(Device, value);
    }

    public bool IsMuted
    {
        get => _isMuted;
        set => SetField(ref _isMuted, value);
    }

    private void DoMuteChanged()
    {
        IsMuted = !IsMuted;
        if (IsMuted)
        {
            _unmutedVolume = VolumePercent;
            _deviceService.SetVolume(0);
        }
        else
        {
            VolumePercent = _unmutedVolume;
            _deviceService.SetVolume(_unmutedVolume);
        }
    }

    private void DoVolumeUpdated()
    {
        _deviceService.SetVolume(VolumePercent);
    }

    public override string ToString()
    {
        return Name;
    }

}
