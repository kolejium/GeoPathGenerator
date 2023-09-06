using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;

namespace GeoPathGenerator.Wpf.Common.Extensions;

public static class ColorExtensions
{
    public static SWMColor ToSWMColor(this SDColor color) => SWMColor.FromArgb(color.A, color.R, color.G, color.B);
    public static SDColor ToSDColor(this SWMColor color) => SDColor.FromArgb(color.A, color.R, color.G, color.B);
}