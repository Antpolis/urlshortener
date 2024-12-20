using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace UrlShortener.Domain.Entities;

[Table("requestLocation")]
public class RequestLocationEntity
{
  [Key]
  [Column("id")]
  public uint ID {get;set;}  

  
  [Column("continent", TypeName = "varchar(5)")]
  public string? ContinentCode {get;set;}

  [Column("continentName", TypeName = "varchar(256)")]
  public string? ContinentName {get;set;}

  [Column("ISOCode", TypeName = "varchar(5)")]
  public string? ISOCode {get;set;}  
  
  [Column("countryName", TypeName = "varchar(256)")]
  public string?  CountryName {get;set;}  

  
  [Column("cityName", TypeName = "varchar(256)")]
  public string? CityName {get;set;}

  
  [Column("subdivision", TypeName = "varchar(256)")]
  public string? Subdivision {get;set;}

  
  [Column("postalCode", TypeName = "varchar(25)")]
  public string? PostalCode {get;set;}

  [Column("location")]
  public Point? Location {get;set;}

  
  [Column("latitude", TypeName = "varchar(12)")]
  public string? Latitude {get;set;}

  
  [Column("longitude", TypeName = "varchar(12)")]
  public string? Longitude {get;set;}  

  
  [Column("requestID")]
  public uint RequestID {get;set;}

  [ForeignKey("RequestID")]
  [Required]
  public RequestEntity Request {get;set;} = null!;
}