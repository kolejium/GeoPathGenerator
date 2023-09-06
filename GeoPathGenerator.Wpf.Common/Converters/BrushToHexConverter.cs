using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GeoPathGenerator.Wpf.Common.Converters;

public class BrushToHexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return null;
        var brush = (SolidColorBrush)value;
        var hex = ToLowerHex(brush.Color.R) +
                  ToLowerHex(brush.Color.G) +
                  ToLowerHex(brush.Color.B);
        return "#" + hex;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static string ToLowerHex(int value)
    {
        return value.ToString("X2").ToLower();
    }
}