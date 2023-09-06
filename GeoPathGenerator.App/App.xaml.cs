using System;
using System.Windows;
using GeoPathGenerator.App.Common.Interfaces;
using GeoPathGenerator.App.Common.Services;
using GeoPathGenerator.App.Infrastructure;
using GeoPathGenerator.App.Managers;
using GeoPathGenerator.App.ViewModels;
using GeoPathGenerator.App.ViewModels.Map;
using GeoPathGenerator.App.ViewModels.Menu;
using GeoPathGenerator.App.ViewModels.Settings;
using GeoPathGenerator.App.Views;
using GeoPathGenerator.App.Views.Map;
using GeoPathGenerator.App.Views.Menu;
using GeoPathGenerator.App.Views.Settings;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;

namespace GeoPathGenerator.App;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register(typeof(ISettingsService), provider => new SettingsService("settings.json"));
        containerRegistry.RegisterSingleton<ApplicationManager>();
        containerRegistry.RegisterSingleton(typeof(SettingsMonitorService), provider => new SettingsMonitorService(provider.Resolve<ISettingsService>(), () => provider.Resolve<ApplicationManager>().Settings, TimeSpan.FromSeconds(2)));

        containerRegistry.RegisterForNavigation<ShellView, ShellViewModel>();
        containerRegistry.RegisterForNavigation<MenuView, MenuViewModel>();
        containerRegistry.RegisterForNavigation<MapControlView, MapControlViewModel>();
        containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        containerRegistry.RegisterForNavigation<EnvironmentSettingsView, EnvironmentSettingsViewModel>();
        containerRegistry.RegisterForNavigation<ThemeSettingsView, ThemeSettingsViewModel>();
        containerRegistry.RegisterForNavigation<MapItemListView, MapItemListViewModel>();
        containerRegistry.RegisterForNavigation<MapItemView, MapItemViewModel>();
        containerRegistry.RegisterForNavigation<MapView, MapViewModel>();
        containerRegistry.RegisterForNavigation<MapToolbarView, MapToolbarViewModel>();
    }

    protected override Window CreateShell()
    {
        Container.Resolve<IRegionManager>().RegisterViewWithRegion(RegionNames.Shell, typeof(ShellView));

        return Container.Resolve<MainWindow>();
    }
}