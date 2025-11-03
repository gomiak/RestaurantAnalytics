using RestaurantAnalytics.Core.Analytics;
using RestaurantAnalytics.Core.Entities.Sales;

namespace RestaurantAnalytics.Core.Interfaces;

public interface ISalesRepository
{
    Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime start, DateTime end);
    Task<decimal> GetAverageTicketAsync(DateTime start, DateTime end);
    Task<Dictionary<DateTime, decimal>> GetDailyRevenueAsync(DateTime start, DateTime end);
    Task<IEnumerable<(string ProductName, double QuantitySold)>> GetTopProductsByQuantityAsync(DateTime start, DateTime end);
    Task<IEnumerable<(string ProductName, decimal TotalSold)>> GetTopProductsByValueAsync(DateTime start, DateTime end);
    Task<IEnumerable<(string ChannelName, decimal TotalRevenue)>> GetTopChannelsByValueAsync(DateTime start, DateTime end);

    Task<IEnumerable<(string ChannelName, int OrderCount)>> GetTopChannelsByQuantityAsync(DateTime start, DateTime end);


    Task<IEnumerable<(string Label, decimal Value)>> RunCustomQueryAsync(
    MetricOption metric,
    DimensionOption dimension,
    DateTime start,
    DateTime end,
    int? storeId,
    int? channelId,
    int? productId
);

    Task<IEnumerable<(int Id, string Name)>> GetStoresAsync();
    Task<IEnumerable<(int Id, string Name, string Type)>> GetChannelsAsync();
    Task<IEnumerable<(int Id, string Name)>> SearchProductsAsync(string search);






}
