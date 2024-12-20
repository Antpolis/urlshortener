using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Domain.Entities;

[Table("account")]
public class AccountEntity {
  [Key]
  [Column("id")]
  public uint ID {get;set;} 
  
  [Column("name", TypeName = "varchar(255)")]
  public string? Name {get;set;}

  [EmailAddress]  
  [Column("contactEmail", TypeName = "varchar(255)")]
  public string? ContactEmail {get;set;}

  [InverseProperty("Account")]
  public ICollection<URLEntity>? Urls {get;set;}
}
