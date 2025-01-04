using Npgsql;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Abstracts;

namespace UrlShortener.Infrastructure.Persistence;


using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public class ApplicationDBContext : DbContext, IApplicationContext
{
    
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    // public ApplicationDBContext() {
    //     //  _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    //
    // }
    public ApplicationDBContext(
        DbContextOptions<ApplicationDBContext> options,  
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
      ) 
        : base(options) {
      _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    
    }

    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder);
    //     builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
    //     Console.WriteLine("testing");
    //     // builder.HasCharSet("utf8mb4", true)
	   //     //  .UseCollation("utf8mb4_general_ci");
    //     
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        
        // string connectionString = connectionBuilder.ConnectionString;
        // Console.WriteLine(connectionString);
        
        // optionsBuilder
            // .UseNpgsql(connectionString );
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // await _mediator.DispatchDomainEvents(this);
        return await base.SaveChangesAsync(cancellationToken);
    }
    
    public void ModifiedEntity<T>(T target, T comparingObj, string[]? includingColumns = null, string[]? ignoringColumns = null) where T : AuditAbstract
    {
        var comparingWithEntity = Entry(comparingObj);
        var fieldUpdated = false;
			 
        foreach (var propertyEntry in Entry(target).Properties)
        {
            var property = propertyEntry.Metadata;
            var ciPropertyName = property.Name.ToUpper();
            // Skip shadow and key properties
            if (property.Name == "CreatedDate" || property.IsShadowProperty() || (propertyEntry.EntityEntry.IsKeySet && property.IsKey())) continue;
				
            if (ignoringColumns is not null && ignoringColumns.Select(a=>a.ToUpper()).Contains(ciPropertyName)) continue;
            if (includingColumns is not null && !includingColumns.Select(a=>a.ToUpper()).Contains(ciPropertyName)) continue;
    
            propertyEntry.CurrentValue = comparingWithEntity.Properties
                .FirstOrDefault(p => p.Metadata.Name == property.Name)?.CurrentValue ?? propertyEntry.CurrentValue;
				
            if (propertyEntry.CurrentValue != propertyEntry.OriginalValue) {
                propertyEntry.IsModified = true;
                fieldUpdated = true;
            }
        }
        if (fieldUpdated) {
            target.LastModifiedDate = DateTime.UtcNow;
        }
    }
    
    public DbSet<AccountEntity> AccountEntity { get; set; }
    public DbSet<DomainEntity> DomainEntity  { get; set; }
    public DbSet<RequestEntity> RequestEntity  { get; set; }
    public DbSet<RequestLocationEntity> RequestLocationEntity  { get; set; }
    public DbSet<TagEntity> TagEntity  { get; set; }
    public DbSet<URLEntity> UrlEntity  { get; set; }
    public DbSet<UserEntity> UserEntity  { get; set; }

}