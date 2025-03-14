using System;

namespace UrlShortener.WebAPI.DTOs;

public record DomainDTO {
  public uint ID { get; set; }
  public string DomainURL { get; set; } = null!;
  public string? DefaultLink { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
