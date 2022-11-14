using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models.AuthModels;
using OnlineGameStore.Common.Enums;
using OnlineGameStore.Extensions;
using System.Threading.Tasks;

namespace OnlineGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserClaims _userClaims;
        public UserController(IUserService userService, UserClaims userClaims)
        {
            _userService = userService;
            _userClaims = userClaims;
        }

        [HttpGet]
        [RoleAuthorize(Role.Administrator)]
        public IActionResult GetAll()
        {
            var users = _userService.GetAllExceptCurrent(_userClaims.Id);
            return Ok(users);
        }


        [HttpGet("{username}")]
        [RoleAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetByUsernameAsync(string username)
        {
            var user = await _userService.GetByUsernameAsync(username);
            return Ok(user);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EditAsync(UserModel updatedUser)
        {
            var user = await _userService.EditAsync(updatedUser);
            return Ok(user);
        }

        [HttpPost("comments/ban")]
        [RoleAuthorize(Role.Administrator, Role.Manager, Role.Moderator)]
        public async Task<IActionResult> BanUserAsync(CommentBanDuration duration)
        {
            await _userService.BanUser(duration);
            return Ok(duration);
        }

        [HttpPost("auth/signUp")]
        public async Task<IActionResult> SignUp(SignUpModel user)
        {

            var token = await _userService.SignUp(user, _userClaims.CookieGuestId);
            return Ok(new { accessTokenKey = token });
        }

        [HttpPost("auth/signIn")]
        public async Task<IActionResult> SignIn(SignInModel user)
        {
            var token = await _userService.SignIn(user, _userClaims.CookieGuestId);
            return Ok(new { accessTokenKey = token });
        }
    }
}
