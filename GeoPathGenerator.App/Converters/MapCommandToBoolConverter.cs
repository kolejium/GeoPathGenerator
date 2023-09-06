using System;
using System.Globalization;
using System.Windows.Data;
using GeoPathGenerator.Wpf.Common.Enums;

namespace GeoPathGenerator.App.Converters;

public class MapCommandToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (Commands)parameter == (Commands?)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? parameter : null;
    }
}