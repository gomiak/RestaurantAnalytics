using RestaurantAnalytics.Core.Analytics;

namespace RestaurantAnalytics.Application.Analytics;

public static class AnalyticsCatalog
{
    public static readonly List<MetricOption> Metrics = new()
    {
        new("faturamento_total", "Faturamento Total", "SUM(s.total_amount)"),
        new("ticket_medio", "Ticket Médio", "AVG(s.total_amount)"),
        new("quantidade_pedidos", "Quantidade de Pedidos", "COUNT(*)")
    };

    public static readonly List<DimensionOption> Dimensions = new()
{
    new("dia", "Dia", "DATE(s.created_at)"),
    new("mes", "Mês", "DATE_TRUNC('month', s.created_at)"),
    new("canal", "Canal", "c.name"),
    new("loja", "Loja", "st.name"),
    new("produto", "Produto", "p.name")
};

}
