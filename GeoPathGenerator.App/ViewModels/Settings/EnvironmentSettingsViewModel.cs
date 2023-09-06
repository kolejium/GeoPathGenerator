using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using GeoPathGenerator.App.Common.Models.Settings;
using GeoPathGenerator.App.Events;
using GeoPathGenerator.App.Managers;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace GeoPathGenerator.App.ViewModels.Settings;

public class EnvironmentSettingsViewModel : BindableBase, INavigationAware
{
    #region [ Variables ]

    private readonly ApplicationManager _applicationManager;
    private string? _fontFamily;

    private uint _fontSize;

    private uint _iconSize;

    private Common.Models.Settings.Settings? _settings;

    #endregion

    #region [ Properties ]

    public IEnumerable<string> AvailableFonts { get; } = new InstalledFontCollection().Families.Select(x => x.Name);

    public string? FontFamily
    {
        get => _fontFamily;
        set => SetProperty(ref _fontFamily, value,
            () =>
            {
                if (_settings != null)
                    _settings.EnvironmentSettings.FontFamily = _fontFamily;
            }); 
    }

    public uint FontSize
    {
        get => _fontSize;
        set => SetProperty(ref _fontSize, Math.Clamp(value, MinFontSize, MaxFontSize),
            () =>
            {
                if (_settings != null)
                    _settings.EnvironmentSettings.FontSize = _fontSize;
            });
    }

    public uint IconSize
    {
        get => _iconSize;
        set => SetProperty(ref _iconSize, Math.Clamp(value, MinIconSize, MaxIconSize),
            () =>
            {
                if (_settings != null)
                    _settings.EnvironmentSettings.IconSize = _iconSize;
            });
    }

    public static uint MaxFontSize => 32;

    public static uint MaxIconSize => 64;

    public static uint MinFontSize => 8;

    public static uint MinIconSize => 8;

    #endregion

    #region [ Constructors ]

    public EnvironmentSettingsViewModel(ApplicationManager applicationManager, IEventAggregator eventAggregator)
    {
        _applicationManager = applicationManager;

        _fontFamily = Common.Models.Settings.Settings.Default.EnvironmentSettings.FontFamily;
        _fontSize = Common.Models.Settings.Settings.Default.EnvironmentSettings.FontSize;

        eventAggregator.GetEvent<SettingsUpdatedEvent>().Subscribe(OnSettingsUpdated);
    }

    #endregion

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.ContainsKey("Settings"))
            _settings = (Common.Models.Settings.Settings)navigationContext.Parameters["Settings"];

        SetSettings(_applicationManager.Settings.EnvironmentSettings);
    }

    private void OnSettingsUpdated(Common.Models.Settings.Settings? obj)
    {
        if (obj != null) SetSettings(obj.EnvironmentSettings);
    }

    private void SetSettings(EnvironmentSettings model)
    {
        FontSize = model.FontSize == 0 ? (uint)Math.Round(SystemFonts.DefaultFont.Size) : model.FontSize;
        FontFamily = string.IsNullOrEmpty(model.FontFamily) || !AvailableFonts.Contains(model.FontFamily)
            ? SystemFonts.DefaultFont.FontFamily.Name
            : model.FontFamily;
        IconSize = model.IconSize == 0 ? EnvironmentSettings.Default.IconSize : model.IconSize;
    }
}