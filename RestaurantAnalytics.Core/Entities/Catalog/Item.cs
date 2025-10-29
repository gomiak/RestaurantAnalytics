namespace RestaurantAnalytics.Core.Entities.Catalog;

public class Item
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = "";
}
