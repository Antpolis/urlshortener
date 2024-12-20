using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController
{
    [HttpGet("testing")]
    public string Testing()
    {
        return "testing";
    }
}