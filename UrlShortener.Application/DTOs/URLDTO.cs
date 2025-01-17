using System;

namespace UrlShortener.Application.DTOs;

public record URLDTO {
  public ulong? ID { get; set; }
  public string FullURL {get;set;}
  public string Hash {get;set;}
  public uint DomainID {get;set;}
  public DateTime CreatedAt {get;set;}
  public DateTime StartDate {get;set;}
  public DateTime EndDate {get;set;}
}
