using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace GeoPathGenerator.App.Behaviors;

public class GroupBindableSelectedItemBehavior : Behavior<FrameworkElement>
{
    #region [ Variables ]

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(GroupBindableSelectedItemBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemPropertyChanged));

    public static readonly DependencyProperty GroupProperty =
        DependencyProperty.RegisterAttached("Group", typeof(string), typeof(BindableSelectedItemBehavior),
            new PropertyMetadata(null, OnGroupPropertyChangedCallback));

    #endregion

    #region [ Properties ]

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    #endregion

    public static bool GetGroup(DependencyObject element)
    {
        return (bool)element.GetValue(GroupProperty);
    }

    public static void SetGroup(DependencyObject element, string value)
    {
        element.SetValue(GroupProperty, value);
    }

    private static void OnGroupPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
    }

    private static void OnSelectedItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {

    }
}