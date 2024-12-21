using MediatR;
using UrlShortener.Application.DTOs;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Commands;

public class CreateAccountCommand: IRequest<AccountEntity> {
    public string? Name {get;set;}
    public string? ContactEmail {get;set;}
}