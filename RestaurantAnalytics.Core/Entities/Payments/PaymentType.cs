namespace RestaurantAnalytics.Core.Entities.Payments;

public class PaymentType
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public string Description { get; set; } = "";
}
