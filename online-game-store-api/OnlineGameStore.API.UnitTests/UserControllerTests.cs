using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models.AuthModels;
using OnlineGameStore.Common.Enums;
using OnlineGameStore.Controllers;
using OnlineGameStore.Extensions;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineGameStore.API.UnitTests
{
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<UserClaims> _mockUserClaims;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();

            var accessor = new Mock<IHttpContextAccessor>();

            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(p => p.IsInRole("Publisher")).Returns(true);

            accessor.Setup(x => x.HttpContext.Request.Cookies["guestId"]).Returns(Guid.NewGuid().ToString());
            accessor.Setup(x => x.HttpContext.User).Returns(() => principal.Object);
            _mockUserClaims = new Mock<UserClaims>(accessor.Object);
        }

        [Test]
        public async Task UserBan_Pass()
        {
            UserController userController = new UserController(_mockUserService.Object, _mockUserClaims.Object);

            await userController.BanUserAsync(CommentBanDuration.OneDay);

            Assert.IsTrue(true);
        }

        [Test]
        public void GetAllAsyncExceptCurrent_ExecOnce()
        {
            UserController userController = new UserController(_mockUserService.Object, _mockUserClaims.Object);

            var actual = userController.GetAll();

            _mockUserService.Verify(x => x.GetAllExceptCurrent(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task GetByUsernameAsync_ExecOnce()
        {
            UserController userController = new UserController(_mockUserService.Object, _mockUserClaims.Object);

            var actual = await userController.GetByUsernameAsync("username");

            _mockUserService.Verify(x => x.GetByUsernameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task SignIn_ExecOnce()
        {
            UserController userController = new UserController(_mockUserService.Object, _mockUserClaims.Object);

            var actual = await userController.SignIn(new SignInModel());

            _mockUserService.Verify(x => x.SignIn(It.IsAny<SignInModel>(), It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task SignUp_ExecOnce()
        {
            UserController userController = new UserController(_mockUserService.Object, _mockUserClaims.Object);

            var actual = await userController.SignUp(new SignUpModel());

            _mockUserService.Verify(x => x.SignUp(It.IsAny<SignUpModel>(), It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task EditAsync_ExecOnce()
        {
            UserController userController = new UserController(_mockUserService.Object, _mockUserClaims.Object);

            var actual = await userController.EditAsync(new UserModel());

            _mockUserService.Verify(x => x.EditAsync(It.IsAny<UserModel>()), Times.Once);
        }
    }
}
