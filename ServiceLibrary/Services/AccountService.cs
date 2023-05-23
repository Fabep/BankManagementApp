using AutoMapper;
using InfrastructureLibrary.Infrastructure.AutoMapper;
using ModelLibrary.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.ViewModels;
using InfrastructureLibrary.Infrastructure.Paging;

namespace ServiceLibrary.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankAppDataContext _bankContext;
        private readonly IMapper _mapper;
        public AccountService(BankAppDataContext bankContext, IMapper mapper)
        {
            _bankContext = bankContext;
            _mapper = mapper;
        }
        public async Task<IQueryable<Account>> GetAccounts()
        {
            return await Task.FromResult(_bankContext.Accounts
                .Include(d => d.Dispositions)
                .ThenInclude(c => c.Customer)
                .Include(t => t.Transactions));
        }

        public async Task<Account> GetAccountById(int id)
        {
            return await _bankContext.Accounts.FindAsync(id);
        }

        public decimal GetAccountBalance(Account targetAccount)
        {
            return targetAccount.Balance;
        }

        public decimal GetAllAccountBalance()
        {
            var allAccounts = GetAccounts().Result.ToList();
            decimal totalBalance = 0;
            allAccounts.ForEach(r => totalBalance += r.Balance);
            return totalBalance;
        }
        public PagedResult<Disposition> GetAccountsFromCustomerId (int customerId, string sortOrder, string sortColumn, string q, int page)
        {
            var query = _bankContext.Dispositions.Include(c => c.Customer).Include(a => a.Account)
                .Include(a => a.Account).ThenInclude(a => a.Loans)
                .Include(a => a.Account).ThenInclude(a => a.Transactions)
                .Include(a => a.Account).ThenInclude(a => a.PermenentOrders)
                .Where(a => a.CustomerId == customerId);

            if (!string.IsNullOrEmpty(q))
            {
                var qSplit = q.Split(' ');
                var qSplitUnique = new HashSet<string>(qSplit);
                foreach (var qForIteration in qSplitUnique)
                {
                    // Search stuff goes here
                }
            }

            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "asc";
            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "AccountId";

            if (sortColumn == "AccountId")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.AccountId);
                else
                    query = query.OrderBy(c => c.AccountId);
            }

            if (sortColumn == "Name")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Customer.Givenname);
                else
                    query = query.OrderBy(c => c.Customer.Givenname);
            }

            if (sortColumn == "Balance")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Account.Balance);
                else
                    query = query.OrderBy(c => c.Account.Balance);
            }

            if (sortColumn == "Frequency")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Account.Frequency);
                else
                {
                    query = query.OrderBy(c => c.Account.Frequency);
                }
            }

            if (sortColumn == "Loans")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Account.Loans.Count);
                else
                {
                    query = query.OrderBy(c => c.Account.Loans.Count);
                }
            }

            if (sortColumn == "PermanentOrders")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Account.PermenentOrders.Count);
                else
                {
                    query = query.OrderBy(c => c.Account.PermenentOrders.Count);
                }
            }
            if (sortColumn == "Transactions")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Account.Transactions.Count);
                else
                {
                    query = query.OrderBy(c => c.Account.Transactions.Count);
                }
            }

            return query.GetPaged(page, 50);

        }
        public List<AccountViewModel> GetTopTenAccounts(string countryCode)
        {
            var disps = _bankContext.Dispositions
                .Include(c => c.Customer)
                .Include(a => a.Account).ThenInclude(a => a.Loans)
                .Include(a => a.Account).ThenInclude(a => a.Transactions)
                .Include(a => a.Account).ThenInclude(a => a.PermenentOrders)
                .Where(d => d.Type.ToLower() == "owner" && countryCode == d.Customer.CountryCode)
                .OrderByDescending(a => a.Account.Balance)
                .Take(10).ToList();
            return _mapper.Map<List<AccountViewModel>>(disps);

        }
        public async Task<Account> CreateAccount(decimal? balance, string? frequency)
        {
            if (balance is null)
            {
                balance = 0;
            }
            if (frequency is null)
            {
                frequency = "Monthly";
            }

            var acc = new Account
            {
                Balance = (decimal)balance,
                Frequency = frequency,
                Created = DateTime.Now
            };

            await _bankContext.Accounts.AddAsync(acc);

            await _bankContext.SaveChangesAsync();

            return acc;
        }

        public bool ExistingAccount(string? transferId)
        {
            if (!int.TryParse(transferId, out int id))
                return false;
            if (_bankContext.Accounts.Find(id) != null)
            {
                return true;
            }
            return false;
                
        }

        public decimal TotalBalance(List<AccountViewModel> accounts)
        {
            return accounts.Select(a => a.Balance).Sum();
        }
    }
}
