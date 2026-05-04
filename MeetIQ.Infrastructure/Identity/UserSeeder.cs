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

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
