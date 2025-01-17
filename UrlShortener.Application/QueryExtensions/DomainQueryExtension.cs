using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.QueryExtensions;

public static class DomainQueryExtension {
  public static IQueryable<DomainEntity> GetByDomain(this IQueryable<DomainEntity> query, string domainURL) {
    return query.Where(d => d.DomainURL == domainURL);
  }

  public static IQueryable<DomainEntity> GetByID(this IQueryable<DomainEntity> query, uint urlID) {
    return query.Where(d=>d.ID == urlID);
  }
}