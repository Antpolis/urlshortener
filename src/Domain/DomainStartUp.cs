using Microsoft.Extensions.DependencyInjection;

namespace urlShortener.Domain;

public static class DomainStartUp
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services;
    }
}