using MeetIQ.Application.Common.Constants;
using MeetIQ.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MeetIQ.Infrastructure.Identity
{
    public static class UserSeeder
    {
        public static async Task SeedAdminAsync(
            UserManager<ApplicationUser> userManager)
        {
            var email = "admin@meetiq.com";
            var password = "Admin@123";
            var fullName = "Boss";

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName
                };

                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
        }
    }
}
