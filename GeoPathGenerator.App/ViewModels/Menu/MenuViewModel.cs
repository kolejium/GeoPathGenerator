using System.Collections.ObjectModel;
using GeoPathGenerator.App.Infrastructure;
using GeoPathGenerator.App.Managers;
using Prism.Mvvm;
using Prism.Regions;

namespace GeoPathGenerator.App.ViewModels.Menu;

public class MenuViewModel : BindableBase
{
    #region [ Variables ]

    private readonly ApplicationManager _applicationManager;
    private readonly IRegionManager _regionManager;

    private MenuItemViewModel _selectedItem;

    #endregion

    #region [ Properties ]

    public ObservableCollection<MenuGroupViewModel> MenuGroups => _applicationManager.MenuGroups;

    public MenuItemViewModel SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value, Navigate);
    }

    #endregion

    #region [ Constructors ]

    public MenuViewModel(ApplicationManager applicationManager, IRegionManager regionManager)
    {
        _applicationManager = applicationManager;
        _regionManager = regionManager;
    }

    #endregion

    private void Navigate()
    {
        if (string.IsNullOrEmpty(SelectedItem.Url))
            return;

        var slashIndex = SelectedItem.Url.IndexOf('/');

        var url = slashIndex > 0 ? SelectedItem.Url.Substring(0, SelectedItem.Url.IndexOf('/')) : SelectedItem.Url;
        url = url.EndsWith("View") ? url : url + "View";

        var parameters = slashIndex > 0
            ? new NavigationParameters
            {
                { "Url", SelectedItem.Url.Remove(0, slashIndex + 1) }
            }
            : null;

        _regionManager.RequestNavigate(RegionNames.MainContent, url, parameters);
    }
}