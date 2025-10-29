﻿namespace RestaurantAnalytics.Core.Entities.Coupons;

public class Coupon
{
    public int Id { get; set; }
    public int BrandId { get; set; }

    public string Code { get; set; } = "";
    public string DiscountType { get; set; } = "";
    public decimal DiscountValue { get; set; }
    public bool IsActive { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
}
