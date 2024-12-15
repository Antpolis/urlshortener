using Microsoft.Extensions.DependencyInjection;

namespace urlShortener.WebAPI;

public static class WebAPIStartUp
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        return services;
    }
}