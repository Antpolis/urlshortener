using UrlShortener.Application.Commands;
using UrlShortener.Domain.Entities;
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

    public static AccountDTO ToAccountDto(this AccountEntity model) {
        return new()
        {
            Name = model.Name,
            ContactEmail = model.ContactEmail
        };
    }
}