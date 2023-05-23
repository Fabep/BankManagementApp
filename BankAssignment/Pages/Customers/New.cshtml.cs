using AutoMapper;
using ServiceLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using BankAssignment.Enumerators;

namespace BankAssignment.Pages.Customers
{
    [Authorize(Roles = "Cashier")]
    [BindProperties]
    public class NewModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IDispositionService _dispositionService;
        private readonly IAccountService _accountService;
        public NewModel(ICustomerService customerService, IDispositionService dispositionService, IAccountService accountService)
        {
            _dispositionService = dispositionService;
            _customerService = customerService;
            _accountService = accountService;

        }
        [MaxLength(100)]
        [Required(ErrorMessage = "Name is required.")]
        public string Givenname { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Name is required.")]
        public string Surname { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Street is required.")]
        public string Streetaddress { get; set; }
        
        [MaxLength(50)]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "Zipcode is required.")]
        public string Zipcode { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        [Range(1, 99, ErrorMessage = "Please choose a valid country.")]
        public _Country Country { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public DateTime Birthday { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string TelephoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Emailaddress { get; set; }

        [Range(1, 99, ErrorMessage = "Please choose a valid gender.")]
        public _Gender Gender { get; set; }
        public List<SelectListItem> Genders { get; set; }

        public void OnGet()
        {
            Countries = GenericEnumHandler.ToSelectListItem(Country);
            Genders = GenericEnumHandler.ToSelectListItem(Gender);
            Birthday = DateTime.Parse("2001-09-11");
        }

        public async Task<IActionResult> OnPost() 
        {
            if (ModelState.IsValid)
            {
                var cus = await _customerService.CreateCustomer(Givenname, Surname, Streetaddress, City, Zipcode, Country.ToString(), Birthday, SocialSecurityNumber, TelephoneNumber, Emailaddress, Gender.ToString());
                
                var acc = await _accountService.CreateAccount(0, "Monthly");
                
                await _dispositionService.AddDisposition( acc.AccountId, cus.CustomerId );

                var message = "Succesfully added customer.";
                return RedirectToPage("Index", new { message });
            }
            Countries = GenericEnumHandler.ToSelectListItem(Country);
            Genders = GenericEnumHandler.ToSelectListItem(Gender);
            return Page();
        }
    }
}
