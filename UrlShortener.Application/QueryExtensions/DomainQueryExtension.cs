using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.QueryExtensions;

public static class DomainQueryExtension {
  public static IQueryable<DomainEntity> GetByName(this IQueryable<DomainEntity> query, string domainName) {
    return query.Where(d => d.Domain == domainName);
  }
}