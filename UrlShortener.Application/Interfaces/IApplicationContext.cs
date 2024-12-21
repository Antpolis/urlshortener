using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Abstracts;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Interfaces;

public interface IApplicationContext
{
    public DbSet<AccountEntity> AccountEntity { get; set; }
    public DbSet<DomainEntity> DomainEntity { get; set; }
    public DbSet<RequestEntity> RequestEntity { get; set; }
    public DbSet<RequestLocationEntity> RequestLocationEntity { get; set; }
    public DbSet<TagEntity> TagEntity { get; set; }
    public DbSet<URLEntity> UrlEntity { get; set; }
    public DbSet<UserEntity> UserEntity { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    public void ModifiedEntity<T>(T target, T comparingObj, string[]? includingColumns = null, string[]? ignoringColumns = null) where T : AuditAbstract;
}