using MediatR;
using UrlShortener.Application.Abstracts;
using UrlShortener.Application.Common;
using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Queries;

public record ListDomainsQuery : QuerySearchParamsAbstract, IRequest<ListResponse<DomainDTO>> {
  public uint? AccountID;
  public bool isPublic = false; 
}
