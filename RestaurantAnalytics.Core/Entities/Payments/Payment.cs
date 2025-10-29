namespace RestaurantAnalytics.Core.Entities.Payments;

public class Payment
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int PaymentTypeId { get; set; }
    public decimal Value { get; set; }
    public bool? IsOnline { get; set; }
}
