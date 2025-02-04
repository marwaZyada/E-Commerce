using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.Api.Extensions
{
    public static class UserManagerExtentionMethod
    {
        public static async Task<AppUser> GetUserAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            var currentuser=await userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.Email==email);
            return currentuser;
        }
    }
}
