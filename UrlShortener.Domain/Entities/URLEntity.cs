using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entities;

[Table("url")]
public class URLEntity
{
  [Key]
  [Column("id")]
  public int ID { get; set; }

  [Column("domainID", TypeName = "int")]
  public int? DomainID { get; set; }

  [Column("redirectURL", TypeName = "text")]
  public string? RedirectURL { get; set; }

  [Column("fullURL", TypeName = "varchar(255)")]
  public string? FullURL { get; set; }

  [Column("accountID", TypeName = "int")]
  public int? AccountID { get; set; }

  [Column("ownerID", TypeName = "int")]
  public int? OwnerID { get; set; }

  [Column("description", TypeName = "text")]
  public string? Description { get; set; }

  [Column("hash", TypeName = "varchar(125)")]
  public string? Hash { get; set; }

  [Column("startDate", TypeName = "datetime")]
  public DateTime? StartDate { get; set; }

  [Column("endDate", TypeName = "datetime")]
  public DateTime? EndDate { get; set; }

  [Column("campaignID", TypeName = "int")]
  public int? CampaignID { get; set; }

  [Column("clientID", TypeName = "int")]
  public int? ClientID { get; set; }

  [ForeignKey("DomainID")]  
  public DomainEntity? Domain { get; set; }

  [ForeignKey("OwnerID")]  
  public AccountEntity? Account { get; set; }
}