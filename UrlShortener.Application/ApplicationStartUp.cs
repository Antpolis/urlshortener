using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Application;

public static class ApplicationStartUp
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        return services;
    }
}