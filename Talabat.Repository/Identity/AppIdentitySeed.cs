using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentitySeed
    {
        public static async Task RoleSeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }
        }
            public static async Task UserSeedAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    Email = "Admin@gmail.com",
                    DisplayName = "Admin",
                    UserName="Adminstrator",
                    PhoneNumber="01208466987"

                };
             await   userManager.CreateAsync(user, "P@ssw0rd");
            }
        }
    }
}
