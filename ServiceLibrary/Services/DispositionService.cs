using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;

namespace ServiceLibrary.Services
{
    public class DispositionService : IDispositionService
    {
        private readonly BankAppDataContext _bankContext;
        public DispositionService(BankAppDataContext bankContext)
        {
            _bankContext = bankContext;
        }

        public async Task AddDisposition(int accountId, int customerId)
        {
            try 
            { 
                var customer = await _bankContext.Customers.FindAsync(customerId);
                var acc = await _bankContext.Accounts.FindAsync(accountId);

                await _bankContext.Dispositions.AddAsync(new Disposition
                {
                    Customer = customer,
                    CustomerId = customer.CustomerId,
                    Account = acc,
                    AccountId = acc.AccountId,
                    Type = "Owner",
                });
                await _bankContext.SaveChangesAsync();
            }
            catch
            {
            }
        }
    }
}
