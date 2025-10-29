namespace RestaurantAnalytics.Core.Entities.Sales;

public class ItemItemProductSale
{
    public int Id { get; set; }
    public int ItemProductSaleId { get; set; }
    public int ItemId { get; set; }
    public int? OptionGroupId { get; set; }
    public int Quantity { get; set; }
    public decimal AdditionalPrice { get; set; }
    public decimal Price { get; set; }
}
