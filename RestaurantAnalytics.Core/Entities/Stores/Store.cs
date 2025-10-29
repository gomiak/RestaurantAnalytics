namespace RestaurantAnalytics.Core.Entities.Stores;

public class Store
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public int SubBrandId { get; set; }
    public string Name { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string District { get; set; } = "";
    public string AddressStreet { get; set; } = "";
    public string AddressNumber { get; set; } = "";
    public bool IsActive { get; set; }
    public bool IsOwn { get; set; }
}
