using Microsoft.AspNetCore.Identity;
using OnlineGameStore.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;
using Role = OnlineGameStore.DAL.Entities.Role;

namespace OnlineGameStore.DAL.Context
{
    internal static class DefaultIdentitySeeding
    {
        public static async Task SeedUsersAsync(UserManager<User> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@mail.com") == null)
            {
                User user = new User
                {
                    UserName = "admin",
                    Email = "admin@mail.com"
                };

                var result = await userManager.CreateAsync(user, "1AdminAdmin!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Common.Enums.Role.Administrator);
                }
            }
        }

        public static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new Role { Name = Common.Enums.Role.User });
                await roleManager.CreateAsync(new Role { Name = Common.Enums.Role.Manager });
                await roleManager.CreateAsync(new Role { Name = Common.Enums.Role.Publisher });
                await roleManager.CreateAsync(new Role { Name = Common.Enums.Role.Moderator });
                await roleManager.CreateAsync(new Role { Name = Common.Enums.Role.Administrator });
            }
        }
    }
}
