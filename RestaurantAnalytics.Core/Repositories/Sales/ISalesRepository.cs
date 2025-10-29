using RestaurantAnalytics.Core.Entities.Sales;

namespace RestaurantAnalytics.Core.Interfaces;

public interface ISalesRepository
{
    Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime start, DateTime end);
    Task<decimal> GetAverageTicketAsync(DateTime start, DateTime end);
    Task<Dictionary<DateTime, decimal>> GetDailyRevenueAsync(DateTime start, DateTime end);
}
