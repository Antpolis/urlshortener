using System.Net;
using MediatR;
using Microsoft.Extensions.Primitives;
using UrlShortener.Application.DTOs;
using UrlShortener.Application.Responses;

namespace UrlShortener.Application.Queries;

public record GetTrafficRedirectQuery: IRequest<GetTrafficRedirectResponse>
{
  public string Hash { get; set; } = null!;
  public string Host { get; set; } = null!;
  public string UserAgent { get; set; } = null!;
  public List<KeyValuePair<string, StringValues>> Headers { get; set; }
  public DateTime RequestDate { get; set; }
  public string? RemoteAddress { get; set; }
  public IPAddress? IP { get; set; }
}