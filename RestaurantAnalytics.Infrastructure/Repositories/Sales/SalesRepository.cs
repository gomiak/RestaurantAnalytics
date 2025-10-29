using Dapper;
using RestaurantAnalytics.Core.Entities.Sales;
using RestaurantAnalytics.Core.Interfaces;
using RestaurantAnalytics.Infrastructure.Database;

namespace RestaurantAnalytics.Infrastructure.Repositories.Sales;

public class SalesRepository : ISalesRepository
{
    private readonly IDbConnectionFactory _factory;

    public SalesRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime start, DateTime end)
    {
        using var conn = await _factory.CreateConnectionAsync();

        var sql = @"
            SELECT 
                id, store_id AS StoreId, channel_id AS ChannelId,
                customer_id AS CustomerId, created_at AS CreatedAt,
                total_amount AS TotalAmount,
                sale_status_desc AS Status
            FROM sales
            WHERE created_at BETWEEN @start AND @end
            ORDER BY created_at;
        ";

        return await conn.QueryAsync<Sale>(sql, new { start, end });
    }

    public async Task<decimal> GetAverageTicketAsync(DateTime start, DateTime end)
    {
        using var conn = await _factory.CreateConnectionAsync();

        var sql = @"
            SELECT COALESCE(AVG(total_amount), 0)
            FROM sales
            WHERE sale_status_desc = 'COMPLETED'
            AND created_at BETWEEN @start AND @end;
        ";

        return await conn.ExecuteScalarAsync<decimal>(sql, new { start, end });
    }

    public async Task<Dictionary<DateTime, decimal>> GetDailyRevenueAsync(DateTime start, DateTime end)
    {
        using var conn = await _factory.CreateConnectionAsync();

        var sql = @"
            SELECT 
                DATE(created_at) AS Date,
                SUM(total_amount) AS Value
            FROM sales
            WHERE sale_status_desc = 'COMPLETED'
            AND created_at BETWEEN @start AND @end
            GROUP BY DATE(created_at)
            ORDER BY DATE(created_at);
        ";

        var rows = await conn.QueryAsync<(DateTime Date, decimal Value)>(sql, new { start, end });
        return rows.ToDictionary(x => x.Date, x => x.Value);
    }
}
