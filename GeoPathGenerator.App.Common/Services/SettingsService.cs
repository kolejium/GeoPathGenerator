using System.Diagnostics;
using GeoPathGenerator.App.Common.Interfaces;
using GeoPathGenerator.App.Common.Models.Settings;
using Newtonsoft.Json;

namespace GeoPathGenerator.App.Common.Services;

public class SettingsService : ISettingsService
{
    #region [ Variables ]

    private readonly string _path;

    #endregion

    #region [ Constructors ]

    public SettingsService(string path)
    {
        _path = path;
    }

    #endregion

    public static Settings? Load(string filePath)
    {
        try
        {
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filePath));
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);

            return null;
        }
    }

    public static async Task<Settings?> LoadAsync(string filePath, CancellationToken token = default)
    {
        try
        {
            return JsonConvert.DeserializeObject<Settings>(await File.ReadAllTextAsync(filePath, token));
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);

            return null;
        }
    }

    public static void Save(string filePath, Settings settings)
    {
        try
        {
            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    public static async Task SaveAsync(string filePath, Settings settings, CancellationToken token = default)
    {
        try
        {
            var json = JsonConvert.SerializeObject(settings);
            await File.WriteAllTextAsync(filePath, json, token);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    public Settings? Load()
    {
        return Load(_path);
    }

    public async Task<Settings?> LoadAsync(CancellationToken token = default)
    {
        return await LoadAsync(_path, token);
    }

    public void Save(Settings settings)
    {
        Save(_path, settings);
    }

    public async Task SaveAsync(Settings settings, CancellationToken token = default)
    {
        await SaveAsync(_path, settings, token);
    }
}