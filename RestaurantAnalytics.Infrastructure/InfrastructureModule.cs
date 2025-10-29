using Microsoft.Extensions.DependencyInjection;
using RestaurantAnalytics.Core.Interfaces;
using RestaurantAnalytics.Infrastructure.Database;
using RestaurantAnalytics.Infrastructure.Repositories.Sales;

namespace RestaurantAnalytics.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        services.AddScoped<ISalesRepository, SalesRepository>();

        return services;
    }
}
