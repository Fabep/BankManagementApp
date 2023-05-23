using Azure.Identity;
using InfrastructureLibrary.Infrastructure.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLibrary.Services;
using System.ComponentModel.DataAnnotations;
using BankAssignment.Enumerators;

namespace BankAssignment.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    [BindProperties]
    public class NewModel : PageModel
    {
        private readonly IAdminService _adminService;
        public NewModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Required]
        [Range(1, 99, ErrorMessage = "Please choose a valid role.")]
        public _Role Role { get; set; }
        public List<SelectListItem> Roles { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordValidation]
        public string Password { get; set; }


        public string? Message { get; set; }
        public void OnGet(string? message)
        {
            Roles = GenericEnumHandler.ToSelectListItem(Role);
            Message = message;
        }

        public async Task<IActionResult> OnPost()
        {
            var message = string.Empty;
            if (ModelState.IsValid)
            {
                message = await _adminService.CreateUser(Role.ToString(), Email, Password);
                return RedirectToPage("Index", new { message });
            }
            Roles = GenericEnumHandler.ToSelectListItem(Role);
            Message = message;
            return Page();
        }
    }
}
