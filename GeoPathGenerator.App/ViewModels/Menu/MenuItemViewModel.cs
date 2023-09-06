using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GeoPathGenerator.App.Common.Models.Menu;
using Prism.Mvvm;

namespace GeoPathGenerator.App.ViewModels.Menu;

public class MenuItemViewModel : BindableBase
{
    #region [ Variables ]

    private string? _icon;
    private string? _title;

    #endregion

    #region [ Properties ]

    public string? Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }

    public string? Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string? Url { get; }

    public ObservableCollection<MenuItemViewModel> Items { get; }

    #endregion

    #region [ Constructors ]

    public MenuItemViewModel(string title, string? icon = null, string? url = null, IEnumerable<MenuItemViewModel>? items = null)
    {
        Title = title;
        Icon = icon;
        Url = url;
        Items = items == null ? null : new ObservableCollection<MenuItemViewModel>(items);
    }

    public MenuItemViewModel(MenuItem model, MenuItem? parent = null) : this(model.Title, model.Icon, model.Url, model.Items.Select(x => new MenuItemViewModel(x)))
    {
        Url = parent == null ? Url : parent.Url + '/' + Url;
    }

    #endregion
}