using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using GeoPathGenerator.App.Common.Interfaces;
using GeoPathGenerator.App.Common.Models.Settings;
using GeoPathGenerator.App.Common.Services;
using GeoPathGenerator.App.Events;
using GeoPathGenerator.App.ViewModels.Menu;
using GeoPathGenerator.Wpf.Common.Managers;

using ImTools;

using MaterialDesignThemes.Wpf;
using Prism.Events;
using Color = System.Drawing.Color;

namespace GeoPathGenerator.App.Managers;

public class ApplicationManager : IDisposable
{
    private readonly SettingsMonitorService _settingsMonitorService;
    private readonly ISettingsService _settingsService;
    private readonly IEventAggregator _eventAggregator;
    private Settings _settings;
    private readonly PaletteHelper _paletteHelper = new();

    public ObservableCollection<MenuGroupViewModel> MenuGroups { get; set; }

    public Theme Theme => (Theme)_paletteHelper.GetTheme();

    public Settings Settings
    {
        get => _settings ?? (Settings) Settings.Default.Clone();
        set
        {
            _settings = value;

            ApplyCustomSettings(_settings.EnvironmentSettings);

            _eventAggregator.GetEvent<SettingsUpdatedEvent>().Publish(_settings);
        }
    }

    private void ApplyCustomSettings(EnvironmentSettings settings)
    {
        CustomSettingsManager.FontFamily = new FontFamily(settings.FontFamily);
        CustomSettingsManager.FontSize = settings.FontSize;
        CustomSettingsManager.IconSize = settings.IconSize;
    }

    private readonly CancellationTokenSource _cancellationTokenSource;

    public ApplicationManager(SettingsMonitorService settingsMonitorService, ISettingsService settingsService, IEventAggregator eventAggregator)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _settingsMonitorService = settingsMonitorService;
        _settingsService = settingsService;
        _eventAggregator = eventAggregator;


        Init();

        _settingsMonitorService.Update += SettingsMonitorServiceOnUpdate;
        _settingsMonitorService.UpdateStarted += SettingsMonitorServiceOnUpdateStarted;
        _settingsMonitorService.UpdateFinished += SettingsMonitorServiceOnUpdateFinished;
        _settingsMonitorService.UpdateCancelled += SettingsMonitorServiceOnUpdateCancelled;

        MenuGroups = new ObservableCollection<MenuGroupViewModel>(new[]
        {
            new MenuGroupViewModel(0, new[]
            {
                new MenuItemViewModel("Map", "Cog", "Map")
            }),
            new MenuGroupViewModel(1, new[]
            {
                new MenuItemViewModel("Settings", "Cog", "Settings", new []
                {
                    new MenuItemViewModel("Environment", "ApplicationCogOutline", "Settings/EnvironmentSettings"),
                    new MenuItemViewModel("Theme", "ThemeLightDark", "Settings/ThemeSettings")
                })
            })
        });
    }

    private void SettingsMonitorServiceOnUpdateCancelled(object? sender, EventArgs e)
    {
        _eventAggregator.GetEvent<SettingsUpdateFinishedEvent>().Publish();
    }

    private void SettingsMonitorServiceOnUpdateFinished(object? sender, EventArgs e)
    {
        _eventAggregator.GetEvent<SettingsUpdateFinishedEvent>().Publish();
    }

    private void SettingsMonitorServiceOnUpdateStarted(object? sender, EventArgs e)
    {
        _eventAggregator.GetEvent<SettingsUpdateStartedEvent>().Publish();
    }

    private void SettingsMonitorServiceOnUpdate(object? sender, Settings e) => Settings = e;

    private void Init()
    {
        var settings = _settingsService.Load();

        if (settings == null)
        {
            settings = new Settings()
            {
                EnvironmentSettings = new EnvironmentSettings()
                {
                    FontFamily = "Arial",
                    FontSize = 16
                },
                ThemeSettings = new ThemeSettings()
                {
                    PrimaryColor = Color.Chocolate, PrimaryForegroundColor = Color.AliceBlue,
                    SecondaryColor = Color.DarkOliveGreen, SecondaryForegroundColor = Color.DeepPink
                }
            };

            _settingsService.Save(settings);
        }

        Settings = settings;
    }

    public void UpdateTheme() => _paletteHelper.SetTheme(Theme);

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}