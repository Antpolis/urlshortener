public class AccountService : RepositoryService<Account>
{
    private readonly IRepository<Account> _repo;

    public AccountService(IRepository<Account> repo) : base(repo)
    {
        _repo = repo;
    }

    public override IQueryable<Account> ListQuery(IQuerySearchParams searchParams)
    {
        return _repo.AsQueryable();
    }

    public override async Task<Account> GetByIdAsync(int id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public override async Task<Account> SaveAsync(Account account)
    {
        return await _repo.SaveAsync(account);
    }

    public override async Task<Account> DeleteAsync(int id)
    {
        var account = await _repo.GetByIdAsync(id);
        if (account != null)
        {
            await _repo.DeleteAsync(account);
        }
        return account;
    }

    public override async Task<Account> RestoreAsync(int id)
    {
        var account = await _repo.GetByIdAsync(id);
        if (account != null)
        {
            await _repo.RestoreAsync(account);
        }
        return account;
    }
}
