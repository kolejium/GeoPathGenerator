using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace GeoPathGenerator.Wpf.Common.Extensions;

public static class VisualTreeHelperExtensions
{
    public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject? parent) where T : DependencyObject
    {
        if (parent == null) yield break;

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            if (child is T typedChild) yield return typedChild;

            foreach (var nestedChild in FindVisualChildren<T>(child)) yield return nestedChild;
        }
    }
}