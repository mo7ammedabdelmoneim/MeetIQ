using MeetIQ.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MeetIQ.Infrastructure.Identity
{
    public static class SeedExtensions
    {
        public static async Task SeedIdentityAsync(this IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            await RoleSeeder.SeedAsync(roleManager);
            await UserSeeder.SeedAdminAsync(userManager);
        }
    }
}
