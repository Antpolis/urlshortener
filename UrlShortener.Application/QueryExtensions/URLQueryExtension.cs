using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.QueryExtensions;

public static partial class URLQueryExtension {
  public static IQueryable<URLEntity> GetByHash(this IQueryable<URLEntity> query, string hash) {
    return query
      .Include(u=>u.Account)
      .Include(u=>u.Domain)
      .Where(d => d.Hash == hash);
  }

  public static IQueryable<URLEntity> GetByID(this IQueryable<URLEntity> query, ulong ID) {
    return query
      .Where(d => d.ID == ID);
  }

  public static IQueryable<URLEntity> ByActive(this IQueryable<URLEntity> query) {
    return query.Where(d=>d.IsActive);
  }
}