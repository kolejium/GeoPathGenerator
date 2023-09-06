using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GeoPathGenerator.App.Common.Interfaces;
using GeoPathGenerator.App.Common.Services;
using GeoPathGenerator.App.Events;
using GeoPathGenerator.App.Infrastructure;
using GeoPathGenerator.App.Managers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace GeoPathGenerator.App.ViewModels.Settings;

public class SettingsViewModel : BindableBase, INavigationAware
{
    private Common.Models.Settings.Settings _settings;
    private readonly ApplicationManager _applicationManager;
    private readonly SettingsMonitorService _settingsMonitorService;
    private readonly ISettingsService _settingsService;
    private readonly IEventAggregator _eventAggregator;
    private readonly IRegionManager _regionManager;

    private CancellationTokenSource _cancellationTokenSource;
    public ICommand SaveCommand { get; }

    public ICommand CancelCommand { get; }

    private readonly ICommand _stopUpdatingCommand;

    private readonly ICommand _startUpdateCommand;

    public ICommand UpdateCommand => IsUpdating ? _stopUpdatingCommand : _startUpdateCommand;


    private bool _autoUpdateEnabled;

    public bool AutoUpdateEnabled
    {
        get => _autoUpdateEnabled;
        set => SetProperty(ref _autoUpdateEnabled, value, async () =>
        {
            if (_autoUpdateEnabled) 
                await _settingsMonitorService.Start().ConfigureAwait(false);
            else
                _settingsMonitorService.Stop();
        });
    }

    private bool _isUpdating;

    public bool IsUpdating
    {
        get => _isUpdating;
        set => SetProperty(ref _isUpdating, value, () =>
        {
            RaisePropertyChanged(nameof(UpdateCommand));
        });
    }

    public SettingsViewModel(ApplicationManager applicationManager, SettingsMonitorService settingsMonitorService, ISettingsService settingsService, IEventAggregator eventAggregator, IRegionManager regionManager)
    {
        _applicationManager = applicationManager;
        _settingsMonitorService = settingsMonitorService;
        _settingsService = settingsService;
        _eventAggregator = eventAggregator;
        _regionManager = regionManager;
        _cancellationTokenSource = new CancellationTokenSource();
        _settings = (Common.Models.Settings.Settings?)_applicationManager.Settings.Clone() ?? Common.Models.Settings.Settings.Default;

        _startUpdateCommand = new DelegateCommand(async () =>
        {
            _eventAggregator.GetEvent<SettingsUpdateStartedEvent>().Publish();

            await Task.Delay(1500); // some work

            _applicationManager.Settings = await _settingsService.LoadAsync(_cancellationTokenSource.Token) ?? Common.Models.Settings.Settings.Default;

            _eventAggregator.GetEvent<SettingsUpdateFinishedEvent>().Publish();
        });

        _stopUpdatingCommand = new DelegateCommand(() =>
        {
            settingsMonitorService.Cancel();
            _cancellationTokenSource?.Cancel();
            _eventAggregator.GetEvent<SettingsUpdateFinishedEvent>().Publish();
        });

        SaveCommand = new DelegateCommand(async () =>
        {
            await _settingsService.SaveAsync(_settings);

            _applicationManager.Settings = _settings;
        });

        CancelCommand = new DelegateCommand(() =>
        {
            _settings = (Common.Models.Settings.Settings)_applicationManager.Settings.Clone();
        });

        _eventAggregator.GetEvent<SettingsUpdateStartedEvent>().Subscribe(OnSettingUpdating);
        _eventAggregator.GetEvent<SettingsUpdateFinishedEvent>().Subscribe(OnSettingsUpdatingFinished);
    }


    private void OnSettingsUpdatingFinished()
    {
        IsUpdating = false;
    }

    private void OnSettingUpdating()
    {
        IsUpdating = true;
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.ContainsKey("Url") && navigationContext.Parameters["Url"].ToString() is var url && !string.IsNullOrEmpty(url))
        {
            url = url.EndsWith("View") ? url : url + "View";

            var parameters = new NavigationParameters()
            {
                { "Settings", _settings }
            };

            _regionManager.RequestNavigate(RegionNames.SettingsContent, url, parameters);
        }
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) => true;

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {

    }
}