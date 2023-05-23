using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public class CountryService : ICountryService
    {
        private readonly BankAppDataContext _bankContext;
        public CountryService(BankAppDataContext bankContext)
        {
            _bankContext = bankContext;
        }
        public List<Customer> GetCustomersCountry(string country)
        {
            return _bankContext.Customers.Where(c => c.Country == country).ToList();
        }

        public List<Account> GetAccountsCountry(string country)
        {
            return _bankContext.Dispositions.Where(d => d.Customer.Country == country && d.Type.ToLower() == "owner").Select(a => a.Account).ToList();
        }
        public decimal GetTotalBalanceCountry(string country)
        {
            return _bankContext.Dispositions.Where(d => d.Customer.Country == country).Sum(a => a.Account.Balance);
        }

    }
}
