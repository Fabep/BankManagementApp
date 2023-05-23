using InfrastructureLibrary.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using BankAssignment.Enumerators;

namespace BankAssignment.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly IAdminService _adminService;


        public EditModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Range(1, 99, ErrorMessage = "Please choose a valid role.")]
        public _Role Role { get; set; }
        public List<SelectListItem> Roles { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [PasswordValidation]
        
        public string Password { get; set; }

        public string UserId { get; set; }


        public void OnGet(string id)
        {
            UserId = id;
            Roles = GenericEnumHandler.ToSelectListItem(Role);
        }
        public async Task<IActionResult> OnPost()
        {
            if (Password == null) 
            { 
                Password = string.Empty;
                ModelState.ClearValidationState(nameof(Password));
                ModelState.MarkFieldValid(nameof(Password));
            }
            if (Email == null)
            {
                Email = string.Empty;
                ModelState.ClearValidationState(nameof(Email));
                ModelState.MarkFieldValid(nameof(Email));
            }


            if(ModelState.IsValid)
            {
                var message = await _adminService.UpdateUser(UserId, Email, Role.ToString(), Password);
                return RedirectToPage("Index", new { message });
            }
            Roles = GenericEnumHandler.ToSelectListItem(Role);
            return Page();
        }
    }
}
