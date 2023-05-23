using ServiceLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLibrary.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;

namespace BankAssignment.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]
    [BindProperties]
    public class TransactionModel : PageModel
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        public TransactionModel(ITransactionService transactionService, IAccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
        }
        public string Type { get; set; }
        public decimal Balance { get; set; }

        [Range(0, 1000000)]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Comment is required.")]
        [MinLength(5, ErrorMessage = "Invalid length, comment to short.")]
        [MaxLength(250, ErrorMessage = "Invalid length, comment to long.")]
        public string Comment { get; set; }
        public string? TransferId { get; set; }
        public string? Message { get; set; }

        public async void OnGet(int id, string type, string? message)
        {
            var account = await _accountService.GetAccountById(id);
            Type = type;
            TransactionDate = DateTime.Now.AddHours(1);
            Balance = account.Balance;
            Message = message;
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var account = await _accountService.GetAccountById(id);
            if (Type == "transfer" || Type == "withdraw" && Amount > _accountService.GetAccountBalance(_accountService.GetAccountById(id).Result))
            {
                ModelState.AddModelError("Amount", "Insufficient balance.");
            }
            if (TransactionDate < DateTime.Now)
            {
                ModelState.AddModelError("TransactionDate", "Transaction required to be in the future.");
            }
            if (Type == "Transfer" && TransferId is null)
            {
                ModelState.AddModelError("TransferId", "Transfer id is required for transfering money.");
            }
            if (Type == "Transfer" && !_accountService.ExistingAccount(TransferId))
            {
                ModelState.AddModelError("TransferId", "Couldn't find account.");
            }
            if (ModelState.IsValid)
            {
                var message = await _transactionService.MakeTransaction(id, Type, Amount, TransferId, TransactionDate, Comment);

                return RedirectToPage("Index", new { id, message });
            }
            Balance = account.Balance;
            return Page();
        }
    }
}
