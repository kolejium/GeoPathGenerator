using System;
using System.Collections.ObjectModel;
using GeoPathGenerator.App.Views.Map;
using GeoPathGenerator.Wpf.Common.Enums;
using Prism.Mvvm;
using Prism.Regions;

namespace GeoPathGenerator.App.ViewModels.Map;

public class MapItemListViewModel : BindableBase, INavigationAware
{
    #region [ Properties ]

    public ObservableCollection<MapItemViewModel> MapItems { get; }

    #endregion

    public bool IsNavigationTarget(NavigationContext navigationContext) => true;

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {

    }

    public MapItemListViewModel()
    {
        MapItems = new ObservableCollection<MapItemViewModel>()
        {
            new MapItemViewModel("Home", MapItemType.Dot),
            new MapItemViewModel("Home2", MapItemType.Dot),
            new MapItemViewModel("Settings", MapItemType.Line)
        };
    }
}