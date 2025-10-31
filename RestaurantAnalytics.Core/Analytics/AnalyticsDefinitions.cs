namespace RestaurantAnalytics.Core.Analytics;

public record MetricOption(string Key, string Label, string SqlExpression);
public record DimensionOption(string Key, string Label, string GroupExpression);
