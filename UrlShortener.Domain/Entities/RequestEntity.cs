using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entities;

[Table("request")]
public class RequestEntity {
  [Key]
  [Column("id", TypeName = "long")]
  public ulong ID {get;set;}

  [Column("URLID", TypeName = "bigint")]
  public ulong? URLID {get;set;}

  [ForeignKey("URLID")]
  public URLEntity? Url {get;set;}

  [Column("browser", TypeName = "varchar(255)")]
  public string? Browser {get;set;}

  [Column("ip", TypeName = "varchar(255)")]
  public string? IP {get;set;}

  [Column("ipv6", TypeName = "varchar(255)")]
  public string? IPv6 {get;set;}

  [Column("rawRequest", TypeName = "text")]
  public string? RawRequest {get;set;}

  [Column("referrer", TypeName = "text")]
  public string? Referrer {get;set;}

  [Column("requestType", TypeName = "varchar(255)")]
  public string? RequestType {get;set;}

  [Column("queryString", TypeName = "varchar(255)")]
  public string? QueryString {get;set;}

  [Column("payload", TypeName = "text")]
  public string? Payload {get;set;}

  [Column("os", TypeName = "varchar(256)")]
  public string? OS {get;set;}

  [Column("agentSource", TypeName = "varchar(256)")]
  public string? AgentSource {get;set;}

  [Column("platform", TypeName = "varchar(256)")]
  public string? Platform {get;set;}

  [Column("requestedDate", TypeName = "datetime")]
  public DateTime? RequestedDate {get;set;}

  [Column("locationID", TypeName = "bigint")]
  public ulong? RequestLocationID {get;set;}

  [ForeignKey("RequestLocationID")]
  public RequestLocationEntity? RequestLocation {get;set;}

  [Column("requestDate", TypeName = "datetime")]
  public DateTime? RequestDate {get;set;}

  [Column("forwardIP", TypeName = "varchar(18)")]
  public string? ForwardIP {get;set;}

  [Column("port", TypeName = "varchar(10)")]
  public string? Port {get;set;}

  [Column("browserVersion", TypeName = "varchar(255)")]
  public string? BrowserVersion {get;set;}

  [Column("isUnique")]
  public bool? IsUnique {get;set;}
}