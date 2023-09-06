using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace GeoPathGenerator.App.Behaviors;

public class BindableSelectedItemBehavior : Behavior<TreeView>
{
    #region [ Variables ]

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(BindableSelectedItemBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemPropertyChanged));

    #endregion

    #region [ Properties ]

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    #endregion

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject != null) AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
    }

    private static void OnSelectedItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not TreeViewItem item) return;

        item.SetValue(TreeViewItem.IsSelectedProperty, true);
    }

    private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        SelectedItem = e.NewValue;
    }
}