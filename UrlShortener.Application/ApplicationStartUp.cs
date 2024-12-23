using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyCSharp.HttpUserAgentParser.DependencyInjection;

namespace UrlShortener.Application;

public static class ApplicationStartUp
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddHttpUserAgentParser();
        return services;
    }
}