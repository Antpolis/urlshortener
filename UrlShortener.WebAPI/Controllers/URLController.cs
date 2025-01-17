using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Commands.Url;
using UrlShortener.Application.DTOs;
using UrlShortener.WebAPI.Requests;

namespace UrlShortener.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    
    private readonly ISender _sender;

    public UrlController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(CreateURLResponse), 201)]
    public async Task<CreateURLResponse> CreateURL([FromBody] CreateURLCommand createURLRequest)
    {
      var urlModel = await _sender.Send(createURLRequest);
      return urlModel;
    }

    [HttpGet("view/{id}")]
    [ProducesResponseType(typeof(URLDTO), 200)]
    [ProducesResponseType(404)]
    public async Task<URLDTO>? GetUrl([FromRoute(Name = "id")] ulong id)
    {
        URLDTO? urlModel = await _sender.Send(new GetURLQuery() {
          ID = id,
        });
        return urlModel;
    }

    [HttpPost("update/{id}")]
    public IActionResult UpdateUrl(int id, [FromBody] Url url)
    {
        var existingUrl = _repository.GetUrl(id);
        if (existingUrl == null)
        {
            return NotFound();
        }
        _repository.UpdateUrl(url);
        return Ok(url);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUrl(int id)
    {
        _repository.DeleteUrl(id);
        return NoContent();
    }
}