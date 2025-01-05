namespace UrlShortener.Application.DTOs;

public record GeoLocation {
  public string? ContinentCode;
  public string? ContinentName;
  public string? ISOCode;
  public string? CountryName;
  public string? CityName;
  public string? PostalCode;
  public string? Longitude;
  public string? Latitude;
  public long? GeoNameId;
}