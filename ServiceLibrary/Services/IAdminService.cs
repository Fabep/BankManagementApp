using Microsoft.AspNetCore.Identity;
using ModelLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public interface IAdminService
    {
        Task<string> CreateUser(string role, string email, string password);
        IdentityUser GetUser(string id);
        Task<IList<string>> GetUserRole(IdentityUser user);
        List<IdentityUser> GetUsers();
        Task<string> UpdateUser(string id, string? email, string? role, string? password);
    }
}
