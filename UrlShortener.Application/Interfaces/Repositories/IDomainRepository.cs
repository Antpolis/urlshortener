using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Interfaces.Repositories;

public interface IDomainRepository
{
    public IQueryable<DomainEntity> List(IQuerySearchParams searchParams);
    public IQueryable<DomainEntity> GetByID(int id);
    public IQueryable<DomainEntity> GetByName(string name);
}