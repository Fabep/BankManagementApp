using ModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public interface ICountryService
    {
        List<Customer> GetCustomersCountry(string country);
        List<Account> GetAccountsCountry(string country);
        decimal GetTotalBalanceCountry(string country);
    }
}
