using Microsoft.AspNetCore.Identity;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Interfaces
{
    public interface IAuthUnitOfWork
    {
        UserManager<User> UserManager { get; }
        SignInManager<User> SignInManager { get; }
        RoleManager<Role> RoleManager { get; }
    }
}
