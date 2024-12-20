using UrlShortener.Application.Interfaces;

namespace UrlShortener.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Domain.Abstracts;
using Domain.Interfaces;



public class AuditableEntitySaveChangesInterceptor: SaveChangesInterceptor {
    private readonly IAuditableUser _auditableUser;
    private readonly IDateTime _dateTime;

    public AuditableEntitySaveChangesInterceptor(IAuditableUser currentUserService, IDateTime dateTime) {
        _auditableUser = currentUserService;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result) {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context) {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<AuditAbstract>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _auditableUser.ID;
                entry.Entity.CreatedDate = _dateTime.Now;
            } 

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = _auditableUser.ID;
                entry.Entity.LastModifiedDate = _dateTime.Now;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}