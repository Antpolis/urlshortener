using MediatR;
using UrlShortener.Application.Commands;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Mappers;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Handlers;

public class CreateAccountHandler(IApplicationContext applicationContext)
    : IRequestHandler<CreateAccountCommand, AccountEntity>
{
    private readonly IApplicationContext _applicationContext = applicationContext;

    public async Task<AccountEntity> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var model = _applicationContext.AccountEntity.Add(request.ToAccountEntity());
        await _applicationContext.SaveChangesAsync(cancellationToken);
        return model.Entity;
    }
}