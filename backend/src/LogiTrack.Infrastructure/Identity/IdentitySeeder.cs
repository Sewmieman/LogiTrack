using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LogiTrack.Infrastructure.Identity;

public static class IdentitySeeder
{
    private static readonly string[] Roles =
    {
        "Admin",
        "Dispatcher",
        "Driver",
        "Customer"
    };

    public static async Task SeedAsync(
        IServiceProvider services)
    {
        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(role));
            }
        }
    }
}