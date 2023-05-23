using ModelLibrary.Models;
using ServiceLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BankAssignment.Pages.Accounts
{
    [AllowAnonymous]
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] {"countryCode"})]
    public class CountryModel : PageModel
    {
        public IAccountService _accountService { get; set; }
        public ICustomerService _customerService { get; set; }
        public CountryModel(IAccountService accountService, ICustomerService customerService)
        {
            _accountService = accountService;
            _customerService = customerService;

        }
        public List<AccountViewModel> TopTenAccounts { get; set; }
        public string Country { get; set; }
        public void OnGet(string countryCode)
        {
            Country = _customerService.GetCountryOrCountryCode(countryCode);
            TopTenAccounts = _accountService.GetTopTenAccounts(countryCode);
        }
    }
}
