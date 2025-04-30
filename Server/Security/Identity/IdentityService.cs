using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Server.Models;
using Server.Models.UserModels;

namespace Server.Security.Identity
{
    public static class IdentityService
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw = "")
        {
            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPw, "superadmin@databruket.se");
                await EnsureRole(serviceProvider, adminID, "superAdmin");

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "admin@databruket.se");
                await EnsureRole(serviceProvider, managerID, "admin");

                var userID = await EnsureUser(serviceProvider, testUserPw, "user@databruket.se");
                await EnsureRole(serviceProvider, userID, "user");

                SeedDB(serviceProvider);
            }
        }

        public async static void SeedDB(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            if (context.Users.Any())
            {
                return;
            }

            var usersToSeed = new[]
            {
        new { UserName = "Hasse", Email = "admin@admin.se", Password = "Admin123!" },
        new { UserName = "Klasse", Email = "admin2@admin.se", Password = "Admin123!" }
    };

            foreach (var u in usersToSeed)
            {
                var user = new User
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, u.Password);
                if (!result.Succeeded)
                {
                    throw new Exception("Seeding user failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(user, "admin"); // or assign based on logic
            }
            context.SaveChanges();
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string userName)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new User
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, testUserPw);
                if (!result.Succeeded)
                {
                    throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
    }
}
