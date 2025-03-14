using System;

namespace UrlShortener.Application.Common;

public record ListResponse<T> {
  public int Total = 0;
  public int Page = 0;
  public required T[] Data;
}
