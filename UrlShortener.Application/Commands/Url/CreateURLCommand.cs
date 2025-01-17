using System;
using MediatR;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Commands.Url;

public class CreateURLCommand: IRequest<CreateURLResponse>
{
  public uint? AccountID { get; set; }
  public string? Email { get; set; }
  public string? Name { get; set; }
  public required string RedirectURL { get; set; }
  public required string Hash { get; set; }
  public required uint DomainID { get; set; }
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
}
