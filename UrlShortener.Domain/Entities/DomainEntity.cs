using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UrlShortener.Domain.Abstracts;

namespace UrlShortener.Domain.Entities;

[Table("domain")]
public class DomainEntity: AuditAbstract {
  [Key]
  [Column("id", TypeName = "integer")]
  public uint ID {get;set;}

  [Column("accountID")]
  public uint? AccountID {get;set;}

  [ForeignKey("AccountID")]
  public virtual AccountEntity? Account {get;set;}

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
