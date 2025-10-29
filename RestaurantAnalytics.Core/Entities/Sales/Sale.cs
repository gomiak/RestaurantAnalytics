namespace RestaurantAnalytics.Core.Entities.Sales;

public class Sale
{
    public int Id { get; set; }

    public int StoreId { get; set; }
    public int ChannelId { get; set; }
    public int? CustomerId { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CustomerName { get; set; } = "";
    public string SaleStatusDesc { get; set; } = "";

    public decimal TotalAmountItems { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalIncrease { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal ServiceTaxFee { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ValuePaid { get; set; }

    public int? ProductionSeconds { get; set; }
    public int? DeliverySeconds { get; set; }

    public int? PeopleQuantity { get; set; }
    public string DiscountReason { get; set; } = "";
    public string Origin { get; set; } = "";
}
