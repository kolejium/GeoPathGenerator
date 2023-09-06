using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Media;
using GeoPathGenerator.App.Common.Interfaces;
using GeoPathGenerator.App.Common.Models.Settings;
using GeoPathGenerator.App.Common.Services;
using GeoPathGenerator.App.Events;
using GeoPathGenerator.App.ViewModels.Menu;
using GeoPathGenerator.Wpf.Common.Extensions;
using GeoPathGenerator.Wpf.Common.Managers;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Events;
using Color = System.Drawing.Color;

namespace GeoPathGenerator.App.Managers;

public class ApplicationManager : IDisposable
{
    #region [ Variables ]

    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IEventAggregator _eventAggregator;
    private readonly PaletteHelper _paletteHelper = new();
    private readonly SettingsMonitorService _settingsMonitorService;
    private readonly ISettingsService _settingsService;
    private Settings _settings;

    #endregion

    #region [ Properties ]

    public ObservableCollection<MenuGroupViewModel> MenuGroups { get; set; }

    public Settings Settings
    {
        get => _settings ?? (Settings)Settings.Default.Clone();
        set
        {
            _settings = value;

            ApplyCustomSettings(_settings.EnvironmentSettings);
            ApplyMaterialTheme(_settings.ThemeSettings);

            _eventAggregator.GetEvent<SettingsUpdatedEvent>().Publish(_settings);
        }
    }

    public Theme Theme => (Theme)_paletteHelper.GetTheme();

    #endregion

    #region [ Constructors ]

    public ApplicationManager(SettingsMonitorService settingsMonitorService, ISettingsService settingsService,
        IEventAggregator eventAggregator)
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
                new MenuItemViewModel("Settings", "Cog", "Settings", new[]
                {
                    new MenuItemViewModel("Environment", "ApplicationCogOutline", "Settings/EnvironmentSettings"),
                    new MenuItemViewModel("Theme", "ThemeLightDark", "Settings/ThemeSettings")
                })
            })
        });
    }

    #endregion

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }

    public void UpdateTheme()
    {
        _paletteHelper.SetTheme(Theme);
    }

    private void ApplyCustomSettings(EnvironmentSettings settings)
    {
        CustomSettingsManager.FontFamily = new FontFamily(settings.FontFamily);
        CustomSettingsManager.FontSize = settings.FontSize;
        CustomSettingsManager.IconSize = settings.IconSize;
    }

    private void ApplyMaterialTheme(ThemeSettings settingsThemeSettings)
    {
        Theme.SetBaseTheme(settingsThemeSettings.IsDark ? Theme.Dark : Theme.Light);

        var primaryColor = settingsThemeSettings.PrimaryColor.ToSWMColor();
        var secondaryColor = settingsThemeSettings.SecondaryColor.ToSWMColor();

        var primaryForegroundColor = settingsThemeSettings.PrimaryForegroundColor.ToSWMColor();
        var secondaryForegroundColor = settingsThemeSettings.SecondaryForegroundColor.ToSWMColor();

        Theme.PrimaryLight = new ColorPair(primaryColor.Lighten(), primaryForegroundColor);
        Theme.PrimaryMid = new ColorPair(primaryColor, primaryForegroundColor);
        Theme.PrimaryDark = new ColorPair(primaryColor.Darken(), primaryForegroundColor);

        Theme.SecondaryLight = new ColorPair(secondaryColor.Lighten(), secondaryForegroundColor);
        Theme.SecondaryMid = new ColorPair(secondaryColor, secondaryForegroundColor);
        Theme.SecondaryDark = new ColorPair(secondaryColor.Darken(), secondaryForegroundColor);

        UpdateTheme();
    }

    private void Init()
    {
        var settings = _settingsService.Load();

        if (settings == null)
        {
            settings = new Settings
            {
                EnvironmentSettings = new EnvironmentSettings
                {
                    FontFamily = "Arial",
                    FontSize = 16
                },
                ThemeSettings = new ThemeSettings
                {
                    IsDark = false,
                    PrimaryColor = Color.Chocolate, PrimaryForegroundColor = Color.AliceBlue,
                    SecondaryColor = Color.DarkOliveGreen, SecondaryForegroundColor = Color.DeepPink
                }
            };

            _settingsService.Save(settings);
        }

        Settings = settings;
    }

    private void SettingsMonitorServiceOnUpdate(object? sender, Settings e)
    {
        Settings = e;
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
}