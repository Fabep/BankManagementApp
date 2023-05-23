using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using ModelLibrary.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ServiceLibrary.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BankAppDataContext _bankContext;
        private readonly IAccountService _accountService;
        public TransactionService(BankAppDataContext bankContext, IAccountService accountService)
        {
            _bankContext = bankContext;
            _accountService = accountService;
        }
        public async Task<List<TransactionViewModel>> GetTransactionsAsync(int id)
        {
            return await _bankContext.Transactions.Where(t => t.AccountId == id).OrderByDescending(t => t.Date).Select(c => new TransactionViewModel
            {
                Id = c.TransactionId,
                Amount = c.Amount,
                Date = DateOnly.FromDateTime(c.Date),
                Type = c.Type,
                AccountId = c.AccountId,
                TransferId = c.Account ?? "-"
            }).ToListAsync();
        }
        public List<TransactionViewModel> GetTransactions(int id) 
        {
            return _bankContext.Transactions.Where(t => t.AccountId == id).OrderByDescending(t => t.Date).Select(c => new TransactionViewModel
            {
                Id = c.TransactionId,
                Amount = c.Amount,
                Date = DateOnly.FromDateTime(c.Date),
                Type = c.Type,
                AccountId = c.AccountId,
                TransferId = c.Account ?? "-"
            }).ToList();
        }

        public async Task<string> MakeTransaction (int id, string type, decimal amount, string? transferId, DateTime transactionDate, string comment)
        {
            var transactionToAdd = new Transaction();
            var transferTarget = new Account();
            if(transferId is not null)
            {
                transferTarget = await _accountService.GetAccountById(Convert.ToInt32(transferId));
            }
            var targetAccount = await _accountService.GetAccountById(id);
            switch(type){
                case "Deposit":
                    transactionToAdd.Type = "Credit";
                    targetAccount.Balance += amount;
                    transactionToAdd.Amount = amount;
                    break;
                case "Withdraw":
                    transactionToAdd.Type = "Debit";
                    targetAccount.Balance -= amount;
                    transactionToAdd.Amount = -amount;
                    break;
                case "Transfer":
                    transactionToAdd.Type = "Debit";
                    targetAccount.Balance -= amount;
                    transactionToAdd.Amount = -amount;
                    transferTarget.Balance += amount;
                    break;
            }
            transactionToAdd.AccountId = id;
            transactionToAdd.Balance = targetAccount.Balance;
            transactionToAdd.Operation = type;
            transactionToAdd.Account = transferId;
            transactionToAdd.Date = transactionDate;
            transactionToAdd.Symbol = comment;

            _bankContext.Transactions.Add(transactionToAdd);

            if (type == "Transfer")
            {
                _bankContext.Transactions.Add(new Transaction
                {
                    AccountId = transferTarget.AccountId,
                    Balance = transferTarget.Balance,
                    Operation = type,
                    Amount = amount,
                    Date = transactionDate,
                    Symbol = comment,
                    Type = "Credit",
                    Account = Convert.ToString(id)
                }) ;

            }

            await _bankContext.SaveChangesAsync();

            return $"{type} succesful.";

        }

    }
}
