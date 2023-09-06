using GeoPathGenerator.App.Common.Interfaces;
using GeoPathGenerator.App.Common.Models.Settings;
using GeoPathGenerator.App.Common.Services;
using Moq;

namespace GeoPathGenerator.App.Common.Tests
{
    public class SettingsMonitorServiceTests
    {
        [Fact]
        public async Task MonitorService_DetectsFileChange()
        {
            // Arrange
            var initialSettings = Settings.Default; // initial settings... we need to have something in the file

            // temp settings file
            var jsonFilePath = Path.GetTempFileName();
            await File.WriteAllTextAsync(jsonFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(initialSettings));

            try
            {
                var settingsService = new SettingsService(jsonFilePath);
                var monitorService = new SettingsMonitorService(settingsService, () => initialSettings, TimeSpan.FromSeconds(2)); // setup
                var changed = false;
                var updated = false;
                var controlReadOperation = false;

                var readCompletionSource = new TaskCompletionSource<bool>();
                var readAfterChangeCompletionSource = new TaskCompletionSource<bool>();

                void MonitorServiceUpdate(object? sender, Settings e) => updated = true;

                void MonitorServiceUpdateStarted(object? sender, EventArgs e)
                {
                    // ReSharper disable once AccessToModifiedClosure
                    if (readCompletionSource.Task.IsCompletedSuccessfully && changed)
                        controlReadOperation = true;
                }

                void MonitorServiceUpdateFinished(object? sender, EventArgs e)
                {
                    readCompletionSource.TrySetResult(true);

                    if (controlReadOperation)
                        readAfterChangeCompletionSource.TrySetResult(true);
                }

                monitorService.Update += MonitorServiceUpdate;
                monitorService.UpdateStarted += MonitorServiceUpdateStarted;
                monitorService.UpdateFinished += MonitorServiceUpdateFinished;

                // Act
                monitorService.Start();

                // Wait read operation
                await readCompletionSource.Task;

                // Assert not changed
                Assert.False(updated, "The file should not be considered changed initially.");

                // Modify settings & save
                var changedSettings = (Settings)Settings.Default.Clone(); // Создайте измененные настройки
                changedSettings.EnvironmentSettings.FontSize = 36;

                await File.WriteAllTextAsync(jsonFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(changedSettings));

                changed = true;

                // Wait read operation after changes
                await readAfterChangeCompletionSource.Task;

                // Assert updated
                Assert.True(updated, "The file should be considered changed after modification.");

                monitorService.UpdateFinished -= MonitorServiceUpdateFinished;
                monitorService.UpdateStarted -= MonitorServiceUpdateStarted;
                monitorService.Update -= MonitorServiceUpdate;

                monitorService.Stop();
            }
            finally
            {
                File.Delete(jsonFilePath);
            }
        }
    }
}