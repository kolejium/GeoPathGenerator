namespace GeoPathGenerator.App.Common.Models.Menu;

public class MenuItem
{
    #region [ Properties ]

    public string Title { get; set; }

    public string? Icon { get; set; }

    public string? Url { get; set; }

    public IEnumerable<MenuItem> Items { get; set; }

    #endregion
}