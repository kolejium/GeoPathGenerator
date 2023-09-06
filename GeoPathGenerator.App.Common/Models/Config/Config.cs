using GeoPathGenerator.App.Common.Models.Menu;

namespace GeoPathGenerator.App.Common.Models.Config;

public partial class Config
{
    public IEnumerable<MenuGroup> MenuGroups { get; set; }

    public Settings.Settings Settings { get; set; }
}