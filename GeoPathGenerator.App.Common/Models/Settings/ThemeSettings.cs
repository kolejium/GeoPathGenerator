using System.Drawing;

namespace GeoPathGenerator.App.Common.Models.Settings;

public class ThemeSettings : ICloneable, IEquatable<ThemeSettings>
{
    public bool IsDark { get; set; }

    public Color PrimaryColor { get; set; }

    public Color SecondaryColor { get; set; }

    public Color PrimaryForegroundColor { get; set; }

    public Color SecondaryForegroundColor { get; set; }

    public object Clone()
    {
        return new ThemeSettings
        {
            IsDark = IsDark,
            PrimaryColor = PrimaryColor,
            SecondaryColor = SecondaryColor,
            PrimaryForegroundColor = PrimaryForegroundColor,
            SecondaryForegroundColor = SecondaryForegroundColor
        };
    }

    public bool Equals(ThemeSettings? other)
    {
        if (other is null) 
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return PrimaryColor.Equals(other.PrimaryColor) && 
               SecondaryColor.Equals(other.SecondaryColor) && 
               PrimaryForegroundColor.Equals(other.PrimaryForegroundColor) && 
               SecondaryForegroundColor.Equals(other.SecondaryForegroundColor);
    }
}