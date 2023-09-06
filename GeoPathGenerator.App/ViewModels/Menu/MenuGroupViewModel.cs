using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GeoPathGenerator.App.Common.Models.Menu;
using Prism.Mvvm;

namespace GeoPathGenerator.App.ViewModels.Menu;

public class MenuGroupViewModel : BindableBase
{
    #region [ Variables ]

    private uint _order;

    #endregion

    #region [ Properties ]

    public ObservableCollection<MenuItemViewModel> Items { get; }

    public uint Order
    {
        get => _order;
        set => SetProperty(ref _order, value);
    }

    #endregion

    #region [ Constructors ]

    public MenuGroupViewModel(uint order, IEnumerable<MenuItemViewModel> items)
    {
        Order = order;
        Items = new ObservableCollection<MenuItemViewModel>(items);
    }

    public MenuGroupViewModel(uint order, IEnumerable<MenuItem> items) : this(order,
        items.Select(x => new MenuItemViewModel(x)))
    {
    }

    public MenuGroupViewModel(MenuGroup model) : this(model.Order, model.Items)
    {
    }

    #endregion
}