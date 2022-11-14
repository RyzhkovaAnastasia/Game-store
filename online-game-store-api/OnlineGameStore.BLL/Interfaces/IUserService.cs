using OnlineGameStore.BLL.Models.AuthModels;
using OnlineGameStore.Common.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface IUserService
    {
        Task<string> SignUp(SignUpModel user, Guid guestId);
        Task<string> SignIn(SignInModel user, Guid guestId);

        IEnumerable<UserModel> GetAllExceptCurrent(Guid id);
        Task<UserModel> GetByIdAsync(Guid id);
        Task<UserModel> GetByUsernameAsync(string username);
        Task<UserModel> EditAsync(UserModel updatedUser);

        Task BanUser(CommentBanDuration duration);
    }
}
