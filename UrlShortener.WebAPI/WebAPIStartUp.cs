using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.WebAPI;

public static class WebAPIStartUp
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        return services;
    }
}