using GeoPathGenerator.App.Common.Models.Settings;

namespace GeoPathGenerator.App.Common.Interfaces;

public interface ISettingsService
{
    Task<Settings?> LoadAsync(CancellationToken token = default);
    Settings? Load();

    Task SaveAsync(Settings settings, CancellationToken token = default);
    void Save(Settings settings);
}