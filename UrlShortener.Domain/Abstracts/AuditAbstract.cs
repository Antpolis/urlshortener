using System.ComponentModel.DataAnnotations.Schema;
using UrlShortener.Domain.Interfaces;

namespace UrlShortener.Domain.Abstracts;

public abstract class AuditAbstract
{
    [Column("createdDate")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedDate { get; set; }

    [Column("createdBy")]
    public Guid? CreatedBy { get; set; }
    
    [Column("lastModifiedDate", TypeName = "timestamp")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? LastModifiedDate { get; set; }

    [Column("lastModifiedBy")]
    public Guid? LastModifiedBy { get; set; }
}