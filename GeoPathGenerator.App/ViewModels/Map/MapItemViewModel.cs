using GeoPathGenerator.Wpf.Common.Enums;
using Prism.Mvvm;

namespace GeoPathGenerator.App.ViewModels.Map;

public class MapItemViewModel : BindableBase
{
    #region [ Variables ]

    private string? _icon;
    private string? _text;

    #endregion

    #region [ Properties ]

    public string? Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }

    public string? Text
    {
        get => _text;
        set => SetProperty(ref _text, value);
    }

    #endregion

    #region [ Constructors ]

    public MapItemViewModel(string text, MapItemType type)
    {
        Text = text;
        Icon = type == MapItemType.Dot ? "CircleSmall" : "Minus";
    }

    #endregion
}