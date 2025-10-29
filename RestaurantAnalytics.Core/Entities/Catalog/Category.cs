namespace RestaurantAnalytics.Core.Entities.Catalog;

public class Category
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
}
