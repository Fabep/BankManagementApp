using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfrastructureLibrary.Infrastructure.Paging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Data.SqlClient;
using AutoMapper;
using ModelLibrary.ViewModels;
using ModelLibrary.Models;
using ServiceLibrary.Services;
using Microsoft.AspNetCore.Authorization;

namespace BankAssignment.Pages.Customers
{
    [Authorize(Roles = "Cashier")]
    public class OverviewModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ISortServices _sortServices;
        public OverviewModel(ICustomerService customerService, IAccountService accountService, IMapper mapper, ISortServices sortServices)
        {
            _customerService = customerService;
            _accountService = accountService;
            _mapper = mapper;
            _sortServices = sortServices;
        }

        public int Id { get; set; }

        public CustomerViewModel Customer { get; set; }

        public List<AccountViewModel> Accounts { get; set; }

        public string Q { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public decimal TotalBalance { get; set; }
        public string? Message { get; set; }
        public async Task OnGet(int id, string? message, string q, string sortOrder, string sortColumn, int pageNo )
        {
            Customer = _mapper.Map<CustomerViewModel>(await _customerService.GetCustomerById(id));
            Message = message;
            Id = id;

            Q = q;
            SortOrder = sortOrder;
            SortColumn = sortColumn;
            if (pageNo == 0)
                pageNo = 1;

            var result = _accountService.GetAccountsFromCustomerId(id, sortOrder, sortColumn, q, pageNo);

            Accounts = _mapper.Map<List<AccountViewModel>>(result.Results);

            TotalBalance = _accountService.TotalBalance(Accounts);
        }
    }
}
