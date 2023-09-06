namespace GeoPathGenerator.App.Common.Models.Settings;

public class EnvironmentSettings : ICloneable, IEquatable<EnvironmentSettings>
{
    public static EnvironmentSettings Default = new()
    {
        IconSize = 16,
        FontFamily = "Arial",
        FontSize = 14
    };

    public string? FontFamily { get; set; }

    public uint FontSize { get; set; }

    public uint IconSize { get; set; }

    public bool Equals(EnvironmentSettings? other)
    {
        if (other == null)
            return false;

        return FontFamily == other.FontFamily && FontSize == other.FontSize && IconSize == other.IconSize;
    }

    public object Clone()
    {
        return new EnvironmentSettings
        {
            FontFamily = FontFamily,
            FontSize = FontSize,
            IconSize = IconSize
        };
    }
}