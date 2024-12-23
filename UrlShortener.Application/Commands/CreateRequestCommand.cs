using MediatR;
using Microsoft.Extensions.Primitives;
using UrlShortener.Application.Responses;

namespace UrlShortener.Application.Commands;

public class CreateRequestCommand: IRequest<CreateRequestResponse> {
  public string Hash { get; set; } = null!;
  public string Host { get; set; } = null!;
  public string UserAgent { get; set; } = null!;
  public List<KeyValuePair<string, StringValues>> Headers { get; set; }
  public DateTime RequestDate { get; set; }
  public ulong UrlEntityID { get; set; }
  public string IPAddress { get; set; }
}