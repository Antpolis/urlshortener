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
        services.AddDbContext<ApplicationDBContext>(options => {
            NpgsqlConnectionStringBuilder connectionBuilder = new NpgsqlConnectionStringBuilder();

            connectionBuilder.Password = configuration["DB:Password"];
            connectionBuilder.Username = configuration["DB:UserName"];
            connectionBuilder.Host= configuration["DB:Host"];
            connectionBuilder.Database = configuration["DB:Name"];
            connectionBuilder.Encoding = "UTF8";
            Console.WriteLine(connectionBuilder.ConnectionString);
            string connectionString = connectionBuilder.ConnectionString;
            options
                .UseNpgsql(connectionString, builder =>
                {
                    builder.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName);
                } );
        });
        
        services.AddScoped<ApplicationDBContext>(p=>p.GetRequiredService<ApplicationDBContext>());
        services.AddTransient<IAuditableUser, UserEntity>();
        
        return services;
    }
}