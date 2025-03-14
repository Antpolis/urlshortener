using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UrlShortener.Domain.Abstracts;
using UrlShortener.Domain.Interfaces;

namespace UrlShortener.Domain.Entities;

[Table("tag")]
public class TagEntity: AuditAbstract {
  
  [Key]
  [Column("id", TypeName = "integer")]
  public uint ID { get; set; }

  [Required]
  [MaxLength(255)]
  [Column("key", TypeName = "varchar(255)")]
  public string Key { get; set; } = null!;
  
  [Required]
  [MaxLength(255)]
  [Column("value", TypeName = "varchar(255)")]
  public string Value { get; set; } = null!;
}
