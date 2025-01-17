using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Commands.Url;
using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.QueryExtensions;

namespace UrlShortener.Application.Handlers.Url;

public class GetURLHandler(IApplicationContext applicationContext) : IRequestHandler<GetURLQuery, URLDTO> {

  private readonly IApplicationContext _applicationContext = applicationContext;
    public async Task<URLDTO>? Handle(GetURLQuery request, CancellationToken cancellationToken) {
      return await _applicationContext.UrlEntity.AsQueryable().GetByID(request.ID).FirstOrDefaultAsync(cancellationToken);
    }
}
