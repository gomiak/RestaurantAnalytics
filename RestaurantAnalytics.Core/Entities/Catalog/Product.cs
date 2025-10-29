namespace RestaurantAnalytics.Core.Entities.Catalog;

public class Product
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public int SubBrandId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = "";
}
