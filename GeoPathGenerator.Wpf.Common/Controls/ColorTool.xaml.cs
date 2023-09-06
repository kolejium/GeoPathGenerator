using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeoPathGenerator.Wpf.Common.Controls;

/// <summary>
///     Логика взаимодействия для ColorTool.xaml
/// </summary>
public partial class ColorTool : UserControl
{
    #region [ Variables ]

    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color?),
        typeof(ColorTool),
        new FrameworkPropertyMetadata(System.Windows.Media.Color.FromRgb(0, 0, 0),
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

    #endregion

    #region [ Properties ]

    public Color? Color
    {
        get => (Color?)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    #endregion

    #region [ Constructors ]

    public ColorTool()
    {
        InitializeComponent();
    }

    #endregion
}