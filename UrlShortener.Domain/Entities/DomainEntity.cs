using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entities;

[Table("domain")]
public class DomainEntity {
  [Key]
  [Column("id", TypeName = "integer")]
  public uint ID {get;set;}

  [ForeignKey("accountID")]
  public uint? AccountID {get;set;}

  [Column("totalShortenURL")]
  public uint TotalShortenURL {get;set;}
  
  [Column("domainURL", TypeName = "varchar(255)")]
  public string? DomainURL {get;set;}

  [Column("system")]
  [DefaultValue(false)]
  public bool IsSystem {get;set;}

  [Column("defaultLink", TypeName = "varchar(1024)")]
  [DefaultValue("https://google.com")]
  public string DefaultLink {get;set;} = null!;

  [InverseProperty("Domain")]
  public ICollection<URLEntity>? Urls {get;set;}
}
