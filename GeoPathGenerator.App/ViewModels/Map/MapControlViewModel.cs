using System.Collections.Generic;
using System.Windows.Input;
using MapControl;
using Prism.Commands;
using Prism.Mvvm;

namespace GeoPathGenerator.App.ViewModels.Map;

public class PointItem
{
    #region [ Properties ]

    public Location Location { get; set; }
    public string Name { get; set; }

    #endregion
}

public class PolylineItem
{
    #region [ Properties ]

    public LocationCollection Locations { get; set; }

    #endregion
}

public class MapControlViewModel : BindableBase
{
    #region [ Properties ]

    public List<PointItem> Points { get; } = new();
    public List<PolylineItem> Polylines { get; } = new();

    #endregion

    #region [ Constructors ]

    public MapControlViewModel()
    {

    }


    #endregion
}