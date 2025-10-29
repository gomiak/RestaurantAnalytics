namespace RestaurantAnalytics.Core.Entities.Coupons;

public class CouponSale
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int CouponId { get; set; }
    public decimal Value { get; set; }
    public string? Sponsorship { get; set; }
}
