using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Interfaces;
using UrlShortener.WebAPI.DTOs;
using UrlShortener.WebAPI.Mappers;

namespace UrlShortener.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController
{

    private readonly ISender _sender;

    public AccountController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet("list")]
    public ICollection<AccountDTO>? List ([FromQuery]IQuerySearchParams searchParams)
    {
        return null;
    }

    [HttpGet("{id}")]
    public async Task<AccountDTO>? Get([FromQuery(Name = "id")] int id)
    {
        return null;
    }

    
    [HttpPost("create")]
    public async Task<AccountDTO>? Create([FromBody] AccountDTO account)
    {
        var model = await _sender.Send(account.ToCreateAccountCommand());
        
    }
    
    [HttpPost("update/{id}")]
    public async Task<AccountDTO> Update([FromBody] AccountDTO account, [FromQuery(Name = "id")] int id) {
        return null;
    }
    
    [HttpPost("restore/{id}")]
    public async Task<AccountDTO> Restore([FromQuery(Name = "id")] int id) {
        return null;
    }

    
    [HttpDelete("delete/{id}")]
    public async Task<AccountDTO> Delete([FromQuery(Name = "id")] int id) {
        return null;
    }
}