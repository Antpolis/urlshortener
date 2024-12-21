using UrlShortener.Application.Commands;
using UrlShortener.Application.DTOs;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Mappers;

public static partial class AccountMapper
{
    public static AccountEntity ToAccountEntity(this AccountDTO model)
    {
        return new AccountEntity()
        {
            Name = model.Name,
            ContactEmail = model.ContactEmail
        };
    }

    public static AccountEntity ToAccountEntity(this CreateAccountCommand model)
    {
        return new AccountEntity()
        {
            Name = model.Name,
            ContactEmail = model.ContactEmail
        };
    }
}