namespace RestaurantAnalytics.Core.Entities.Delivery;

public class DeliverySale
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public string CourierName { get; set; } = "";
    public string CourierPhone { get; set; } = "";
    public string CourierType { get; set; } = "";
    public string DeliveryType { get; set; } = "";
    public string Status { get; set; } = "";
    public decimal DeliveryFee { get; set; }
    public decimal CourierFee { get; set; }
}
