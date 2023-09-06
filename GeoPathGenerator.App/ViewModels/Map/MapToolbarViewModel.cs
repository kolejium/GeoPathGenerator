using GeoPathGenerator.Wpf.Common.Enums;
using Prism.Mvvm;

namespace GeoPathGenerator.App.ViewModels.Map;

public class MapToolbarViewModel : BindableBase
{
    #region [ Variables ]

    private Commands? _toolCommand;

    #endregion

    #region [ Properties ]

    public Commands? ToolCommand
    {
        get => _toolCommand;
        set => SetProperty(ref _toolCommand, value);
    }

    #endregion
}