using System.Windows.Controls;
using MapControl;

namespace GeoPathGenerator.App.Views.Map;

/// <summary>
///     Логика взаимодействия для MapView.xaml
/// </summary>
public partial class MapControlView : UserControl
{
    #region [ Constructors ]

    public MapControlView()
    {
        InitializeComponent();

        ImageLoader.HttpClient.DefaultRequestHeaders.Add("User-Agent",
            $"GeoPathGenerator/{typeof(App).Assembly.GetName().Version}");
    }

    #endregion
}