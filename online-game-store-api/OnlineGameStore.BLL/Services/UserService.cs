using AutoMapper;
using Microsoft.Extensions.Options;
using OnlineGameStore.BLL.Configurations.JWT;
using OnlineGameStore.BLL.CustomExceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models.AuthModels;
using OnlineGameStore.Common.Enums;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthUnitOfWork _authUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IOptions<AuthOptions> _options;
        private readonly IOrderService _orderService;
        public UserService(IAuthUnitOfWork authUnitOfWork, IMapper mapper, IOptions<AuthOptions> options, IOrderService orderService)
        {
            _authUnitOfWork = authUnitOfWork;
            _mapper = mapper;
            _options = options;
            _orderService = orderService;
        }

        public async Task<string> SignUp(SignUpModel user, Guid guestId)
        {
            User userEntity = new User { Email = user.Email, UserName = user.Username, SecurityStamp = Guid.NewGuid().ToString() };

            await CheckUniqueValues(userEntity);

            var result = await _authUnitOfWork.UserManager.CreateAsync(userEntity, user.Password);
            if (result.Succeeded)
            {
                await _orderService.MergeBasketsAsync(guestId, userEntity.Id);
                await _authUnitOfWork.UserManager.AddToRoleAsync(userEntity, Common.Enums.Role.User);
                await _authUnitOfWork.SignInManager.SignInAsync(userEntity, true);
                return new JWT(_options).GenerateJWT(_mapper.Map<UserModel>(await GetByUsernameAsync(userEntity.UserName)));
            }

            throw new ModelException(string.Join(',', result));
        }

        public async Task<string> SignIn(SignInModel user, Guid guestId)
        {
            var userEntity = await _authUnitOfWork.UserManager.FindByEmailAsync(user.Email);
            if (userEntity == null)
            {
                throw new ModelException("Invalid email or password");
            }
            var signInResult = await _authUnitOfWork.SignInManager.PasswordSignInAsync(userEntity, user.Password, true, false);
            if (!signInResult.Succeeded)
            {
                throw new ModelException("Invalid email or password");
            }
            await _orderService.MergeBasketsAsync(guestId, userEntity.Id);
            return new JWT(_options).GenerateJWT(_mapper.Map<UserModel>(await GetByUsernameAsync(userEntity.UserName)));
        }

        public async Task SignOut()
        {
            await _authUnitOfWork.SignInManager.SignOutAsync();
        }

        public IEnumerable<UserModel> GetAllExceptCurrent(Guid id)
        {
            var users = _authUnitOfWork.UserManager.Users.Where(x => x.Id != id).ToList();
            users.ForEach(x => x.Roles = _authUnitOfWork.UserManager.GetRolesAsync(x).Result);
            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<UserModel> GetByIdAsync(Guid id)
        {
            var user = _authUnitOfWork.UserManager.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                throw new NotFoundException();
            }
            user.Roles = await _authUnitOfWork.UserManager.GetRolesAsync(user);
            return _mapper.Map<UserModel>(user);
        }


        public Task BanUser(CommentBanDuration duration)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> GetByUsernameAsync(string username)
        {
            var user = await _authUnitOfWork.UserManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new NotFoundException();
            }
            user.Roles = await _authUnitOfWork.UserManager.GetRolesAsync(user);
            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> EditAsync(UserModel updatedUser)
        {
            var user = _authUnitOfWork.UserManager.Users.FirstOrDefault(x => x.Id == updatedUser.Id);
            if (user == null)
            {
                throw new NotFoundException();
            }

            user.Email = updatedUser.Email;
            user.UserName = updatedUser.Username;
            user.PublisherId = updatedUser.Role == Common.Enums.Role.Publisher ? updatedUser.PublisherId : null;

            await CheckUniqueValues(user);

            var result = await _authUnitOfWork.UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new ModelException(string.Join("\n", result.Errors));
            }

            await EditRoleAsync(user, updatedUser.Role);
            return _mapper.Map<UserModel>(user);
        }

        public async Task EditRoleAsync(User user, string roleName)
        {
            user.Roles = await _authUnitOfWork.UserManager.GetRolesAsync(user);

            await _authUnitOfWork.UserManager.RemoveFromRolesAsync(user, user.Roles);
            await _authUnitOfWork.UserManager.AddToRoleAsync(user, roleName);
        }

        private async Task CheckUniqueValues(User user)
        {
            var userByEmail = await _authUnitOfWork.UserManager.FindByEmailAsync(user.Email);
            if (userByEmail != null && user.Id != userByEmail.Id)
            {
                throw new ModelException("Email already registered");
            }

            var userByName = await _authUnitOfWork.UserManager.FindByNameAsync(user.UserName);
            if (userByName != null && user.Id != userByName.Id)
            {
                throw new ModelException("Username already exist");
            }
        }
    }
}
