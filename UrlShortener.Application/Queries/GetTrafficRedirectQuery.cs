using MediatR;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Queries;

public class GetTrafficRedirectQuery: IRequest<URLEntity>
{
    public string Hash { get; set; } = null!;
}