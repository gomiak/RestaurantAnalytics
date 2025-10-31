using Dapper;
using RestaurantAnalytics.Core.Analytics;
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

    public async Task<IEnumerable<(int Id, string Name)>> GetStoresAsync()
    {
        using var conn = await _factory.CreateConnectionAsync();

        var sql = @"
        SELECT id, name 
        FROM stores
        WHERE is_active = true
        ORDER BY name;
    ";

        return await conn.QueryAsync<(int Id, string Name)>(sql);
    }



    public async Task<IEnumerable<(string Label, decimal Value)>> RunCustomQueryAsync(
    MetricOption metric,
    DimensionOption dimension,
    DateTime start,
    DateTime end,
    int? storeId,
    int? channelId,
    int? productId)
    {
        using var conn = await _factory.CreateConnectionAsync();

        string groupField = dimension.Key switch
        {
            "dia" => "TO_CHAR(DATE(s.created_at), 'DD/MM/YY')",
            "mes" => "TO_CHAR(DATE_TRUNC('month', s.created_at), 'MM/YY')",
            _ => dimension.GroupExpression
        };

        string sql = $@"
        SELECT 
            {groupField} AS Label,
            {metric.SqlExpression} AS Value
        FROM sales s
        LEFT JOIN stores st   ON st.id = s.store_id
        LEFT JOIN channels c  ON c.id = s.channel_id
        WHERE s.created_at BETWEEN @start AND @end
        AND (@storeId IS NULL OR s.store_id = @storeId)
        AND (@channelId IS NULL OR s.channel_id = @channelId)
        AND (
            @productId IS NULL OR EXISTS (
                SELECT 1 FROM product_sales ps 
                WHERE ps.sale_id = s.id AND ps.product_id = @productId
            )
        )
        GROUP BY {groupField}
        ORDER BY MIN(s.created_at);
    ";

        return await conn.QueryAsync<(string Label, decimal Value)>(sql, new
        {
            start,
            end,
            storeId,
            channelId,
            productId
        });
    }

    public async Task<IEnumerable<(int Id, string Name, string Type)>> GetChannelsAsync()
    {
        using var conn = await _factory.CreateConnectionAsync();

        var sql = @"
        SELECT id, name, type
        FROM channels
        ORDER BY name;
    ";

        return await conn.QueryAsync<(int Id, string Name, string Type)>(sql);
    }

    public async Task<IEnumerable<(int Id, string Name)>> SearchProductsAsync(string search)
    {
        using var conn = await _factory.CreateConnectionAsync();

        var sql = @"
        SELECT id, name 
        FROM products
        WHERE deleted_at IS NULL
        AND name ILIKE @pattern
        ORDER BY name
        LIMIT 20;
    ";

        return await conn.QueryAsync<(int Id, string Name)>(sql, new { pattern = $"%{search}%" });
    }


}
