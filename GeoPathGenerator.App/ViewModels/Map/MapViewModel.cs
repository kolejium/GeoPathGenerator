using GeoPathGenerator.App.Infrastructure;
using GeoPathGenerator.App.Views;
using GeoPathGenerator.App.Views.Map;
using Prism.Mvvm;
using Prism.Regions;

using System.Collections.ObjectModel;
using System.Diagnostics;
using GeoPathGenerator.Wpf.Common.Enums;
using System.Drawing;

namespace GeoPathGenerator.App.ViewModels.Map;

public class MapViewModel : BindableBase, INavigationAware
{
    #region [ Variables ]

    private readonly IRegionManager _regionManager;

    #endregion

    public MapToolbarViewModel ToolbarViewModel { get; }

    #region [ Constructors ]

    public MapViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;

        _regionManager.RequestNavigate(RegionNames.Map, nameof(MapControlView));
        _regionManager.RequestNavigate(RegionNames.MapItemList, nameof(MapItemListView));

        ToolbarViewModel = new MapToolbarViewModel();
    }

    #endregion

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _regionManager.AddToRegion(RegionNames.Map, nameof(MapControlView));
        _regionManager.AddToRegion(RegionNames.MapItemList, nameof(MapItemListView));
        _regionManager.AddToRegion(RegionNames.MapToolbar, nameof(MapToolbarView));
    }
}