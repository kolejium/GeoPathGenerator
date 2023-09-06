using GeoPathGenerator.App.Common.Interfaces;
using GeoPathGenerator.App.Common.Models.Settings;

namespace GeoPathGenerator.App.Common.Services;

public class SettingsMonitorService
{
    #region [ Variables ]

    private readonly Func<Settings> _funcToGetComparable;
    private readonly ISettingsService _service;
    private readonly TimeSpan _timeSpan;

    private CancellationTokenSource _cancellationTokenSource;
    private PeriodicTimer? _periodicTimer;

    #endregion

    #region [ Events ]

    public event EventHandler<Settings>? Update;

    public event EventHandler? UpdateCancelled;

    public event EventHandler? UpdateFinished;

    public event EventHandler? UpdateStarted;

    #endregion

    #region [ Constructors ]

    public SettingsMonitorService(ISettingsService service, Func<Settings> funcToGetComparable, TimeSpan timeSpan)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _service = service;
        _funcToGetComparable = funcToGetComparable;
        _timeSpan = timeSpan;
    }

    #endregion

    public void Cancel()
    {
        _cancellationTokenSource.Cancel();

        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void CancelCurrentOperation()
    {
        _cancellationTokenSource.Cancel();
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel(true);
        _cancellationTokenSource.Dispose();
        _periodicTimer?.Dispose();
    }

    public async Task Start(CancellationToken token = default)
    {
        _periodicTimer = new PeriodicTimer(_timeSpan);

        while (await _periodicTimer.WaitForNextTickAsync(token))
        {
            if (token.IsCancellationRequested)
                break;

            var currentToken = _cancellationTokenSource.Token;

            try
            {
                await Execute(currentToken);
            }
            catch (OperationCanceledException)
            {
                _cancellationTokenSource = new CancellationTokenSource();

                UpdateCancelled?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
        _periodicTimer?.Dispose();
    }

    private async Task Execute(CancellationToken token = default)
    {
        UpdateStarted?.Invoke(this, EventArgs.Empty);

        await Task.Delay(1500, token); // some work

        var oldSettings = _funcToGetComparable();
        var settings = await _service.LoadAsync(token);

        if (settings != null && !settings.Equals(oldSettings))
            Update?.Invoke(this, settings);

        UpdateFinished?.Invoke(this, EventArgs.Empty);
    }
}