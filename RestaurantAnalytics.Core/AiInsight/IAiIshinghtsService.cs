namespace RestaurantAnalytics.Core.Interfaces;

public interface IAiInsightsService
{
    Task<string> GenerateInsightAsync(DateTime start, DateTime end, CancellationToken ct = default);

    Task<string> GenerateTrendInsightAsync(Dictionary<DateTime, decimal> dailyRevenue);
    Task<string> GenerateLabelValueInsightAsync(
          IEnumerable<(string Label, decimal Value)> data,
          string metricLabel,
          string dimensionLabel,
          CancellationToken ct = default);

    // Task<string> GenerateProductInsightsAsync(DateTime start, DateTime end, int topN = 10, CancellationToken ct = default);
    // Task<string> GenerateChannelInsightsAsync(DateTime start, DateTime end, CancellationToken ct = default);
}
