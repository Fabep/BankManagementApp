using InfrastructureLibrary.Infrastructure.Paging;
using ModelLibrary.Models;
using ModelLibrary.ViewModels;

namespace ServiceLibrary.Services
{
    public interface IAccountService
    {
        Task<IQueryable<Account>> GetAccounts();

        Task<Account> GetAccountById(int id);

        decimal GetAccountBalance(Account targetAccount);

        decimal GetAllAccountBalance();
        PagedResult<Disposition> GetAccountsFromCustomerId(int customerId, string sortOrder, string sortColumn, string q, int page);

        List<AccountViewModel> GetTopTenAccounts(string countryCode);
        Task<Account> CreateAccount(decimal? balance, string? frequency);
        bool ExistingAccount(string? transferId);
        decimal TotalBalance(List<AccountViewModel> accounts);
    }
}
