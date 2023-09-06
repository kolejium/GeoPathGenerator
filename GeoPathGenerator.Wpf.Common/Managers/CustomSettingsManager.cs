using System.Windows;
using System.Windows.Media;

namespace GeoPathGenerator.Wpf.Common.Managers;

public static class CustomSettingsManager
{
    #region [ Properties ]

    public static FontFamily FontFamily
    {
        get => (FontFamily)Application.Current.Resources["FontFamily"];
        set => Application.Current.Resources["FontFamily"] = value;
    }

    public static double FontSize
    {
        get => (double)Application.Current.Resources["FontSize"];
        set => Application.Current.Resources["FontSize"] = value;
    }

    public static double IconSize
    {
        get => (double)Application.Current.Resources["IconSize"];
        set => Application.Current.Resources["IconSize"] = value;
    }


    #endregion
}