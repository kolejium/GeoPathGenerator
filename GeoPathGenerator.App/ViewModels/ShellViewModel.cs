using GeoPathGenerator.App.Infrastructure;
using Prism.Mvvm;
using Prism.Regions;

namespace GeoPathGenerator.App.ViewModels;

public class ShellViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;

    public ShellViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;

        _regionManager.RegisterViewWithRegion(RegionNames.Menu, "MenuView");
    }
}