using ServiceLibrary.Services;
using ModelLibrary;
using ModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using ModelLibrary.ViewModels;

namespace SuspiciousMoney
{
    public class Application
    {
        private readonly ICountryService _countryService;
        private readonly ITransactionService _transactionService;
        private readonly ICustomerService _customerService;
        public Application(ICountryService countryService, ITransactionService transactionService, ICustomerService customerService)
        {
            _transactionService = transactionService;
            _countryService = countryService;
            _customerService = customerService;
        }
        private string[] Countries = new string[] { "Sweden", "Denmark", "Norway", "Finland" };
        public void Run()
        {
            foreach (var country in Countries)
            {
                var fileName = $"../../../Reports/Sussy-{country}.txt";
                var lastChecked = string.Empty;
                try
                {
                    lastChecked = File.ReadLines(fileName).SkipLast(1).Last();
                    lastChecked = lastChecked.Split(' ')[1];
                }
                catch (FileNotFoundException) { }

                if (lastChecked == string.Empty)
                {
                    lastChecked = DateTime.MinValue.ToString();
                }
                var accountList = _countryService.GetAccountsCountry(country);

                foreach (var account in accountList)
                {
                    var toFile = new List<string>();
                    var transactions = _transactionService
                    .GetTransactions(account.AccountId)
                        .Where(d =>
                        DateTime.Parse(lastChecked) <= d.Date.ToDateTime(TimeOnly.MinValue) &&
                        d.Date.ToDateTime(TimeOnly.MinValue) <= DateTime.UtcNow).ToArray();

                    var cusForAccount = _customerService.GetCustomersFromAccount(account).First();
                    if (transactions.Length > 0)
                    {
                        foreach (var transaction in transactions)
                        {
                            if (Math.Abs(transaction.Amount) > 15000)
                            {
                                toFile.Add($"Amount: {transaction.Amount}kr");
                                toFile.Add($"Transaction: {transaction.Id}");
                                toFile.Add($"--------------------------------------------------------------");
                            }
                        }
                        if (transactions.Where(t => t.Date.ToDateTime(TimeOnly.MinValue) > DateTime.UtcNow.AddDays(-3)).Sum(d => Math.Abs(d.Amount)) > 23000)
                        {
                            toFile.Add($"Comment: Total transactions exceed 23000kr.");
                            toFile.Add($"--------------------------------------------------------------");
                        }
                    }
                    if (toFile.Count > 0)
                    {
                        toFile.Insert(0, $"Customer: {cusForAccount.Surname} {cusForAccount.Givenname}");
                        toFile.Insert(1, $"Account: {account.AccountId}");
                        File.AppendAllLines(fileName, toFile);
                    }
                }
                File.AppendAllText(fileName, $"BatchDate: {DateOnly.FromDateTime(DateTime.UtcNow)} {Environment.NewLine}" +
                    $"--------------------------------------------------------------{Environment.NewLine}");
            }
        }
    }
}
