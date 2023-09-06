namespace GeoPathGenerator.App.Common.Models.Settings;

public class Settings : ICloneable, IEquatable<Settings>
{
    public static readonly Settings Default = new()
    {
        EnvironmentSettings = EnvironmentSettings.Default,
        ThemeSettings = new ThemeSettings()
    };

    #region [ Properties ]

    public EnvironmentSettings EnvironmentSettings { get; set; } = new();

    public ThemeSettings ThemeSettings { get; set; } = new();

    #endregion

    public object Clone()
    {
        return new Settings()
        {
            EnvironmentSettings = (EnvironmentSettings)EnvironmentSettings.Clone(),
            ThemeSettings = (ThemeSettings)ThemeSettings.Clone()
        };
    }

    public bool Equals(Settings? other)
    {
        if (other is null) 
            return false;

        if (ReferenceEquals(this, other)) 
            return true;

        return EnvironmentSettings.Equals(other.EnvironmentSettings) && ThemeSettings.Equals(other.ThemeSettings);
    }
}