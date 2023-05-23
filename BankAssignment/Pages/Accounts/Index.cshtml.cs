using ModelLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLibrary.ViewModels;
using ServiceLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using InfrastructureLibrary.Infrastructure;
using InfrastructureLibrary.Infrastructure.Paging;
using AutoMapper;

namespace BankAssignment.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public IndexModel(IAccountService accountService, ITransactionService transactionService, IMapper mapper)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _mapper = mapper;
        }

        public AccountViewModel Account { get; set; }
        public int AccountId { get; set; }
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetShowMore(int pageNo, int id)
        {
            var awaitTransactions = await _transactionService.GetTransactionsAsync(id);
            var tHolder = awaitTransactions.AsQueryable().GetPaged(pageNo, 20);
            var transactionList = tHolder.Results.ToList();

            return new JsonResult(new { transactions = transactionList });
        }
        public async Task OnGet(int id, string? message)
        {
            var acc = await _accountService.GetAccountById(id);
            Account = _mapper.Map<AccountViewModel>(acc);
            AccountId = Account.Id;
            Message = message;
        }
    }
}
