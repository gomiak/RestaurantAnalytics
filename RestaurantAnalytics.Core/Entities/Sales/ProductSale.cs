namespace RestaurantAnalytics.Core.Entities.Sales;

public class ProductSale
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal BasePrice { get; set; }
    public decimal TotalPrice { get; set; }
}
