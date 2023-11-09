using Domain.Entities;
using Domain.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class AppDbContextSeed
    {
        public static async Task EnsureCreate(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<AppDbContext>();

            await dbContext.Database.EnsureCreatedAsync();
        }

        public static async Task SeedDefaultUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var userRole in Enum.GetValues<UserRole>())
            {
                var role = new IdentityRole(userRole.ToString());
                if (roleManager.Roles.All(x => x.Name != role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            var defaultUser = new List<(AppUser, string, UserRole)>()
            {
                (new AppUser()
                {
                    UserName = "master@gmail.com",
                    Email = "master@gmail.com",
                    RootUser = new RootUser() { FirstName = "Master", LastName = "Account" }
                }, "123123", UserRole.Master),
                (new AppUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    RootUser = new RootUser() { FirstName = "Admin", LastName = "Account" }
                }, "123123", UserRole.Admin),
                (new AppUser()
                {
                    UserName = "user@gmail.com",
                    Email = "user@gmail.com",
                    RootUser = new RootUser() { FirstName = "Default User", LastName = "Account" }
                }, "123123", UserRole.User)
            };

            foreach (var user in defaultUser.Where(user =>
                         !userManager.Users.Any(x => x.UserName == user.Item1.UserName)))
            {
                await userManager.CreateAsync(user.Item1, user.Item2);
                await userManager.AddToRoleAsync(user.Item1, user.Item3.ToString());
            }
        }
    }
}