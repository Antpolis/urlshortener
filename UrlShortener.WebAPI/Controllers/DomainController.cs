using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Queries;
using UrlShortener.WebAPI.DTOs;
using UrlShortener.WebAPI.Mappers;

namespace UrlShortener.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DomainController(ISender sender) {
  private readonly ISender _sender = sender;

  [HttpGet("list")]
  public async Task<ICollection<DomainDTO>>? List ([FromQuery]ListDomainsQuery searchParams) {
    var response = await this._sender.Send(searchParams);
    return response.Data.Select(x => x.ToDomainDTO()).ToList();
  }
}