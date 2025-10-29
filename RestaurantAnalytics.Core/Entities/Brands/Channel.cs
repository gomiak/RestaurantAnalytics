namespace RestaurantAnalytics.Core.Entities.Brands;

public class Channel
{
    public int Id { get; set; }
    public int BrandId { get; set; }

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Type { get; set; } = "";
}
