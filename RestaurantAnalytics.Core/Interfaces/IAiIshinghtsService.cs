namespace RestaurantAnalytics.Core.Interfaces;

public interface IAiInsightsService
{
    /// <summary>
    /// Gera um insight textual (em PT-BR) com base no faturamento do período.
    /// </summary>
    Task<string> GenerateInsightAsync(DateTime start, DateTime end, CancellationToken ct = default);

    Task<string> GenerateTrendInsightAsync(Dictionary<DateTime, decimal> dailyRevenue);



    // Deixe estes prontos para evoluir depois (opcionais):
    // Task<string> GenerateProductInsightsAsync(DateTime start, DateTime end, int topN = 10, CancellationToken ct = default);
    // Task<string> GenerateChannelInsightsAsync(DateTime start, DateTime end, CancellationToken ct = default);
}
