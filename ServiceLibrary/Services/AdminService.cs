using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ModelLibrary.Models;
using System.Runtime.CompilerServices;

namespace ServiceLibrary.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AdminService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IdentityUser GetUser(string id)
        {
            return _userManager.Users.First(x => x.Id == id);
        }

        public List<IdentityUser> GetUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<string> UpdateUser(string id, string? email, string? role, string? password)
        {
            var userToUpdate = GetUser(id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(userToUpdate);


            if (email != string.Empty)
            {
                await _userManager.SetEmailAsync(userToUpdate, email);
                await _userManager.SetUserNameAsync(userToUpdate, email);
            }


            if (password != string.Empty)
               await _userManager.ResetPasswordAsync(userToUpdate, token, password);

            try
            {
                if (role.ToLower() == "admin" || role.ToLower() == "cashier")
                {
                    await _userManager.RemoveFromRolesAsync(userToUpdate, new List<string> { "Admin", "Cashier" });
                    await _userManager.AddToRoleAsync(userToUpdate, role);
                }
                return "Succesfully updated user.";
            }
            catch
            {
                return "Couldn't update user.";
            }
        }
        public async Task<string> CreateUser(string role, string email, string password)
        {
            if (role is null || password is null || email is null) return "At least one of the parameters were null.";
            if (_userManager.FindByEmailAsync(email).Result != null) return "User not found.";
            var user = new IdentityUser
            {
                Email = email,
                UserName = email,
                EmailConfirmed = true
            };
            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, role);
            return "Succesfully created user.";
        }

        public async Task<IList<string>> GetUserRole(IdentityUser user)
        {
            try
            {
                return await _userManager.GetRolesAsync(user);
            }
            catch
            {
                return new List<string> { "-" };
            }
        }
    }
}
