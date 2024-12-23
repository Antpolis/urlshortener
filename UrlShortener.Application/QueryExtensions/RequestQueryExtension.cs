using System;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.QueryExtensions;

public static partial class RequestQueryExtension {
  public static IQueryable<RequestEntity> ByUrlID(this IQueryable<RequestEntity> query, ulong urlID) {
    return query.Where(d=>d.URLID == urlID);
  }

  public static IQueryable<RequestEntity> ByDateRange(this IQueryable<RequestEntity> query, DateTime startDate, DateTime endDate) {
    return query.Where(d=>d.RequestedDate >= startDate && d.RequestedDate <= endDate);
  }
}
