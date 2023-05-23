using BankAssignment.Enumerators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLibrary.Services;
using System.ComponentModel.DataAnnotations;

namespace BankAssignment.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]
    [BindProperties]
    public class NewModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IDispositionService _dispositionService;
        public NewModel(IDispositionService dispositionService, IAccountService accountService)
        {
            _accountService = accountService;
            _dispositionService = dispositionService;
        }

        [Range(1, 99, ErrorMessage = "Choose a valid frequency.")]
        public _Frequency Frequency { get; set; }
        public List<SelectListItem> Frequencies { get; set; }
        public decimal Balance { get; set; }

        public int Id { get; set; }

        public void OnGet(int id)
        {
            Id = id;
            Frequencies = GenericEnumHandler.ToSelectListItem(Frequency);
        }
        public async Task<IActionResult> OnPost(int id)
        {
            if (Balance < 0)
                ModelState.AddModelError("Balance", "Balance can't be less than zero.");

            if (ModelState.IsValid)
            {
                var acc = await _accountService.CreateAccount(Balance, Frequency.ToString());

                await _dispositionService.AddDisposition(acc.AccountId, id);

                var message = "Succesfully created account.";
                return RedirectToPage("/Customers/Overview", new { id, message });
            }

            Frequencies = GenericEnumHandler.ToSelectListItem(Frequency);
            return Page();
        }
    }
}
