using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLibrary.ViewModels;
using ServiceLibrary.Services;

namespace BankAssignment.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IAdminService _adminService;
        public IndexModel(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public List<UserViewModel> Users { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGet(string? message)
        {
            Message = message;
            Users = _adminService.GetUsers().Select(a => new UserViewModel
            {
                Id = a.Id,
                Email = a.Email,
                Role = _adminService.GetUserRole(a).Result[0],
            }).ToList();
            return Page();
        }
    }
}
