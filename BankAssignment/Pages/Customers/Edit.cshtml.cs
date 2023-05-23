using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ServiceLibrary.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using BankAssignment.Enumerators;

namespace BankAssignment.Pages.Customers
{
    [Authorize(Roles = "Cashier")]
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IUtilityService _utilityService;
        public EditModel(ICustomerService customerService, IUtilityService utilityService)
        {
            _customerService = customerService;
            _utilityService = utilityService;
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

        [MaxLength(20)]
        public string SocialSecurityNumber { get; set; }

        [MaxLength(20)]
        public string TelephoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Emailaddress { get; set; }

        [Range(1, 99, ErrorMessage = "Please choose a valid gender.")]
        public _Gender Gender { get; set; }
        public List<SelectListItem> Genders { get; set; }

        public int CustomerId { get; set; }
        public async void OnGet(int id)
        {
            Genders = GenericEnumHandler.ToSelectListItem(Gender);
            Countries= GenericEnumHandler.ToSelectListItem(Country);
            CustomerId = id;
            var cus = await _customerService.GetCustomerById(CustomerId);
            Givenname = cus.Givenname;
            Surname = cus.Surname;
            Streetaddress = cus.Streetaddress;
            City = cus.City;
            Zipcode = cus.Zipcode;
            Country = (_Country)Enum.Parse(typeof(_Country), _utilityService.ToTitleCase(cus.Country));
            Birthday = (DateTime)cus.Birthday!;
            SocialSecurityNumber = cus.NationalId;
            TelephoneNumber = cus.Telephonenumber;
            Emailaddress = cus.Emailaddress;
            Gender = (_Gender)Enum.Parse(typeof(_Gender), _utilityService.ToTitleCase(cus.Gender));
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var message = await _customerService.UpdateCustomer(CustomerId, Givenname, Surname, Streetaddress, City, Zipcode, Country.ToString(), Birthday, SocialSecurityNumber, TelephoneNumber, Emailaddress, Gender.ToString());
                var id = CustomerId;
                return RedirectToPage("/Customers/Overview", new { id, message });
            }
            Countries = GenericEnumHandler.ToSelectListItem(Country);
            Genders = GenericEnumHandler.ToSelectListItem(Gender);
            return Page();
        }
    }
}
