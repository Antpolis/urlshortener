using UrlShortener.Application.Commands;
using UrlShortener.WebAPI.DTOs;

namespace UrlShortener.WebAPI.Mappers;

public static partial class AccountMapper
{
    public static CreateAccountCommand ToCreateAccountCommand(this AccountDTO model)
    {
        return new()
        {
            Name = model.Name,
            ContactEmail = model.ContactEmail
        };
    }
}