
using Microsoft.AspNetCore.Identity;
using OnlineGameStore.DAL.Context;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Repositories
{
    public class AuthUnitOfWork : IAuthUnitOfWork
    {
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        public RoleManager<Role> RoleManager { get; }

        public AuthUnitOfWork(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;

            DefaultIdentitySeeding.SeedRolesAsync(roleManager).Wait();
            DefaultIdentitySeeding.SeedUsersAsync(userManager).Wait();
        }
    }
}
