using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Services;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Interfaces;

namespace UrlShortener.Infrastructure;

using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

public static class InfrastructureStartUp
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IAuditableUser, UserEntity>();
        services.AddDbContext<ApplicationDBContext>();
        //
        // services.AddScoped<ApplicationDBContext>(p=>p.GetRequiredService<ApplicationDBContext>());
        
        
        return services;
    }
}