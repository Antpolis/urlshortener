using System;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Commands.Url;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Mappers;
using UrlShortener.Application.QueryExtensions;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Handlers.Url;

public class CreateURLHandler(IApplicationContext applicationContext): IRequestHandler<CreateURLCommand, CreateURLResponse>{
  private readonly IApplicationContext _applicationContext = applicationContext;
  public async Task<CreateURLResponse> Handle(CreateURLCommand request, CancellationToken cancellationToken) {

    var accountModel = new AccountEntity() {
      ContactEmail = request.Email,
      Name = request.Name
    };
      
    var accountCount = await _applicationContext.AccountEntity
        .AsQueryable()
        .Where(a=> (request.AccountID != null && a.ID == request.AccountID) || (request.Email != null && a.ContactEmail == request.Email))
        .CountAsync(cancellationToken);
    if(accountCount == 0) {
      accountModel = _applicationContext.AccountEntity.Add(accountModel).Entity;
      await _applicationContext.SaveChangesAsync(cancellationToken);
    } else {
      accountModel = await _applicationContext.AccountEntity
        .AsQueryable()
        .Where(a=> (request.AccountID != null && a.ID == request.AccountID) || (request.Email != null && a.ContactEmail == request.Email))
        .FirstOrDefaultAsync(cancellationToken);
    }
    var urlModel = request.ToURLEntity();
    // check domainID exists
    // Generate hash if req.hash is empty
    var domainModel = await _applicationContext.DomainEntity.AsQueryable().GetByID(request.DomainID).FirstOrDefaultAsync(cancellationToken);
    if(domainModel == null) {
      throw new Exception("Domain not found");
    }
    // Generate the fullUrl, which is domainUrl + Hash
    urlModel.FullURL = domainModel.DomainURL + "/" + request.Hash;
    urlModel.AccountID = accountModel.ID;
    await _applicationContext.SaveChangesAsync(cancellationToken);

    return urlModel.ToCreateURLResponse();
  }
}
