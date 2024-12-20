using UrlShortener.Domain.Abstracts;

namespace UrlShortener.Application.Interfaces;

public interface IApplicationContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    public void ModifiedEntity<T>(T target, T comparingObj, string[]? includingColumns = null, string[]? ignoringColumns = null) where T : AuditAbstract;
}