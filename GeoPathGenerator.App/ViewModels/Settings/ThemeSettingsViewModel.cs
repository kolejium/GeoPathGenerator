using System;
using System.Windows.Input;
using System.Windows.Media;

using GeoPathGenerator.App.Common.Models.Settings;
using GeoPathGenerator.App.Events;
using GeoPathGenerator.App.Managers;
using GeoPathGenerator.Wpf.Common.Enums;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace GeoPathGenerator.App.ViewModels.Settings;

public class ThemeSettingsViewModel : BindableBase, INavigationAware
{
    #region [ Variables ]

    private readonly ApplicationManager _applicationManager;

    private Common.Models.Settings.Settings? _settings;

    private ColorScheme _activeScheme;

    private Color? _color;
    private bool _isDarkTheme;

    private Color? _primaryColor;

    private Color? _primaryForegroundColor;

    private Color? _secondaryColor;

    private Color? _secondaryForegroundColor;

    #endregion

    #region [ Properties ]

    public ColorScheme ActiveScheme
    {
        get => _activeScheme;
        set => SetProperty(ref _activeScheme, value);
    }

    public ICommand ChangeToPrimaryCommand { get; }

    public ICommand ChangeToPrimaryForegroundCommand { get; set; }

    public ICommand ChangeToSecondaryCommand { get; set; }

    public ICommand ChangeToSecondaryForegroundCommand { get; set; }

    public Color? Color
    {
        get => _color;
        set => SetProperty(ref _color, value, () =>
        {
            var currentSchemeColor = ActiveScheme switch
            {
                ColorScheme.Primary => _primaryColor,
                ColorScheme.Secondary => _secondaryColor,
                ColorScheme.PrimaryForeground => _primaryForegroundColor,
                ColorScheme.SecondaryForeground => _secondaryForegroundColor,
                _ => throw new NotSupportedException(
                    $"{ActiveScheme} is not a handled ColorScheme.. Ye daft programmer!")
            };

            if (_color != currentSchemeColor) ChangeCustomColor(value);
        });
    }

    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set
        {
            if (SetProperty(ref _isDarkTheme, value))
            {
                _applicationManager.Theme.SetBaseTheme(value ? Theme.Dark : Theme.Light);
                _applicationManager.UpdateTheme();

                if (_settings != null)
                    _settings.ThemeSettings.IsDark = _isDarkTheme;
            }
        }
    }

    #endregion

    #region [ Constructors ]

    public ThemeSettingsViewModel(ApplicationManager applicationManager, IEventAggregator eventAggregator)
    {
        _applicationManager = applicationManager;
        _color = Colors.Black;

        ChangeToPrimaryCommand = new DelegateCommand(() => ChangeScheme(ColorScheme.Primary));
        ChangeToSecondaryCommand = new DelegateCommand(() => ChangeScheme(ColorScheme.Secondary));
        ChangeToPrimaryForegroundCommand = new DelegateCommand(() => ChangeScheme(ColorScheme.PrimaryForeground));
        ChangeToSecondaryForegroundCommand = new DelegateCommand(() => ChangeScheme(ColorScheme.SecondaryForeground));

        eventAggregator.GetEvent<SettingsUpdatedEvent>().Subscribe(OnSettingsUpdated);
    }

    private void OnSettingsUpdated(Common.Models.Settings.Settings? obj)
    {
        if (obj != null) SetSettings(obj.ThemeSettings);
    }

    #endregion

    private void ChangeCustomColor(Color? color)
    {
        if (color == null)
            return;

        var value = color.Value;

        switch (ActiveScheme)
        {
            case ColorScheme.Primary:
                _applicationManager.Theme.PrimaryLight = new ColorPair(value.Lighten());
                _applicationManager.Theme.PrimaryMid = new ColorPair(value);
                _applicationManager.Theme.PrimaryDark = new ColorPair(value.Darken());

                _applicationManager.UpdateTheme();

                _primaryColor = color;
                break;
            case ColorScheme.Secondary:
                _applicationManager.Theme.SecondaryLight = new ColorPair(value.Lighten());
                _applicationManager.Theme.SecondaryMid = new ColorPair(value);
                _applicationManager.Theme.SecondaryDark = new ColorPair(value.Darken());

                _applicationManager.UpdateTheme();

                _secondaryColor = color;
                break;
            case ColorScheme.PrimaryForeground:
                SetPrimaryForegroundToSingleColor(value);
                _primaryForegroundColor = color;
                break;
            case ColorScheme.SecondaryForeground:
                SetSecondaryForegroundToSingleColor(value);
                _secondaryForegroundColor = color;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeScheme(ColorScheme scheme)
    {
        ActiveScheme = scheme;
        Color = ActiveScheme switch
        {
            ColorScheme.Primary => _applicationManager.Theme.PrimaryMid.Color,
            ColorScheme.Secondary => _applicationManager.Theme.SecondaryMid.Color,
            ColorScheme.PrimaryForeground => _primaryForegroundColor,
            ColorScheme.SecondaryForeground => _secondaryForegroundColor,
            _ => Color
        };

        var theme = _applicationManager.Theme;
    }

    private void SetPrimaryForegroundToSingleColor(Color color)
    {
        _applicationManager.Theme.PrimaryLight = new ColorPair(_applicationManager.Theme.PrimaryLight.Color, color);
        _applicationManager.Theme.PrimaryMid = new ColorPair(_applicationManager.Theme.PrimaryMid.Color, color);
        _applicationManager.Theme.PrimaryDark = new ColorPair(_applicationManager.Theme.PrimaryDark.Color, color);

        _applicationManager.UpdateTheme();
    }

    private void SetSecondaryForegroundToSingleColor(Color color)
    {
        _applicationManager.Theme.SecondaryLight = new ColorPair(_applicationManager.Theme.SecondaryLight.Color, color);
        _applicationManager.Theme.SecondaryMid = new ColorPair(_applicationManager.Theme.SecondaryMid.Color, color);
        _applicationManager.Theme.SecondaryDark = new ColorPair(_applicationManager.Theme.SecondaryDark.Color, color);

        _applicationManager.UpdateTheme();
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.ContainsKey("Settings"))
            _settings = (Common.Models.Settings.Settings)navigationContext.Parameters["Settings"];

        SetSettings(_applicationManager.Settings.ThemeSettings);
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {

    }

    private void SetSettings(ThemeSettings model)
    {
        IsDarkTheme = model.IsDark;
    }
}