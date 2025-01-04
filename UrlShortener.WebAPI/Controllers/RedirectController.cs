using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Queries;

namespace UrlShortener.WebAPI.Controllers;

[Microsoft.AspNetCore.Components.Route("")]
public class RedirectController {

  private readonly ISender _sender;
  
  public RedirectController(ISender sender) {
    _sender = sender;
  }
  
  [HttpGet("{hash}")]
  public async Task<RedirectResult> RedirectTraffic([FromQuery] string hash, [FromHeader] string host, HttpRequest request) {
    var senderModel = new GetTrafficRedirectQuery()
    {
      Host = host,
      Hash = hash,
      UserAgent = request.Headers.UserAgent.ToString(),
      RequestDate = new DateTime(),
      Headers = request.Headers.ToList()
    };
    
    var response = await _sender.Send(senderModel);
    return new RedirectResult(response.RedirectUrl, response.PermRedirect);
  }
}