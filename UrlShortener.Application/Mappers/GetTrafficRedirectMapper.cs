using UrlShortener.Application.Commands;
using UrlShortener.Application.Queries;

namespace UrlShortener.Application.Mappers;

public static class GetTrafficRedirectMapper {
  public static CreateRequestCommand ToCreateRequest(this GetTrafficRedirectQuery model, ulong urlEntityID, string IPAddress) {
    return new()
    {
      Hash = model.Hash,
      Host = model.Host,
      UserAgent = model.UserAgent,
      Headers = model.Headers,
      RequestDate = model.RequestDate,
      UrlEntityID = urlEntityID,
      IPAddress = IPAddress
    };
  } 
}