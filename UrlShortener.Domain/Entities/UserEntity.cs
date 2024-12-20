using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UrlShortener.Domain.Interfaces;

namespace UrlShortener.Domain.Entities;

[Table("user")]
public class UserEntity: IAuditableUser
{
  [Column("id")]
  [Key]  
  public Guid ID {get;set;}

  [Column("password", TypeName = "varchar(255)")]
  [Required]
  public string Password {get;set;} = null!;

  [Column("passwordHash", TypeName = "varchar(255)")]
  [Required]
  public string PasswordHash {get;set;} = null!;

  [Column("email", TypeName = "varchar(255)")]
  [Required]
  public string Email {get;set;} = null!;

  [Column("code", TypeName = "varchar(255)")]
  public string? Code {get;set;}

  [Column("codeExpire", TypeName = "datetime")]
  public DateTime? CodeExpire {get;set;}
}