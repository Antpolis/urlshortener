using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Queries;

namespace UrlShortener.WebAPI.Controllers;

[Controller]
[Route("")]
public class RedirectController: ControllerBase{

  private readonly ISender _sender;
  
  public RedirectController(ISender sender) {
    _sender = sender;
  }
  
  [HttpGet("{hash}")]
  public async Task<RedirectResult> RedirectTraffic([FromQuery] string hash, [FromHeader] string host) {
    var senderModel = new GetTrafficRedirectQuery()
    {
      Host = host,
      Hash = hash,
      UserAgent = Request.Headers.UserAgent.ToString(),
      RequestDate = new DateTime(),
      Headers = Request.Headers.ToList(),
      IP = HttpContext.Connection.RemoteIpAddress 
    };
    
    
    var response = await _sender.Send(senderModel);
    return new RedirectResult(response.RedirectUrl, response.PermRedirect);
  }
}