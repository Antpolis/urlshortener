using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Domain;

public static class DomainStartUp
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services;
    }
}