using System.Runtime.InteropServices.JavaScript;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Interfaces.Repositories;

public interface IAccountRepository {
  public IQueryable<AccountEntity> GetByPrimaryKeyQuery(int id); 
  public IQueryable<AccountEntity> GetAccountByUserID(int userID); 
  public IQueryable<AccountEntity> GetAccountByName(string name); 
  public IQueryable<AccountEntity> GetAccountByEmail(string email);
  public AccountEntity Save(AccountEntity account);
  public AccountEntity Delete(int id);
  public AccountEntity Delete(AccountEntity account);
  public AccountEntity Restore(int id);
  public IQueryable<AccountEntity> List(IQuerySearchParams searchParams);
}