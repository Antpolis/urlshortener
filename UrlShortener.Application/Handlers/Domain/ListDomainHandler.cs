using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using UrlShortener.Application.Common;
using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Mappers;
using UrlShortener.Application.Queries;
using UrlShortener.Application.QueryExtensions;

namespace UrlShortener.Application.Handlers.Domain;

public  class ListDomainHandler(IApplicationContext applicationContext) : IRequestHandler<ListDomainsQuery, ListResponse<DomainDTO>> {
  private readonly IApplicationContext _applicationContext = applicationContext;
  public async Task<ListResponse<DomainDTO>> Handle(ListDomainsQuery request, CancellationToken cancellationToken)
  {
    var query = _applicationContext.DomainEntity.AsQueryable();
    if(request.AccountID.HasValue) {
      query = query.GetByAccountID(request.AccountID.Value);
    }
    if(request.isPublic) {
      query = query.GetBySystem(request.isPublic);
    }

    var domainModels = await query.ToListAsync(cancellationToken);
    return new ListResponse<DomainDTO> {
      Data = domainModels.Select(d => d.ToDomainDTO()).ToArray(),
      Total = domainModels.Count,
    };
  }
}
