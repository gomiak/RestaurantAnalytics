namespace RestaurantAnalytics.Core.Interfaces
{
    public interface IAiInsightsService
    {
        Task<string> GenerateTrendInsightAsync(Dictionary<string, decimal> data);
        Task<string> GenerateLabelValueInsightAsync(
            IEnumerable<(string Label, decimal Value)> data,
            string metricLabel,
            string dimensionLabel,
            CancellationToken ct = default
        );
    }
}
