using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.CustomExceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Controllers;
using OnlineGameStore.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineGameStore.API.UnitTests
{
    public class GameControllerTests
    {
        private Mock<IWebHostEnvironment> _mockEnvironment;
        private Mock<IGameService> _mockGameService;
        private Mock<UserClaims> _mockUserClaims;
        private Guid userId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _mockGameService = new Mock<IGameService>();
            var accessor = new Mock<IHttpContextAccessor>();

            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(p => p.IsInRole("Publisher")).Returns(true);
            accessor.Setup(x => x.HttpContext.Request.Cookies["guestId"]).Returns(userId.ToString());
            accessor.Setup(x => x.HttpContext.User).Returns(() => principal.Object);
            _mockUserClaims = new Mock<UserClaims>(accessor.Object);

        }

        [Test]
        public async Task GetAllAsyncGamesAsync_ExecOnce()
        {
            _mockGameService.Setup(x => x.GetAllAsync()).ReturnsAsync(() => new List<GameModel>());
            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);

            var actual = await gameController.GetAllAsync();

            _mockGameService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetByKeyAsync_ValidKey_ExecOnce()
        {
            _mockGameService.Setup(x => x.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync(() => new GameModel());
            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);

            await gameController.GetGameByKeyAsync("");

            _mockGameService.Verify(x => x.GetByKeyAsync(""), Times.Once);
        }

        [Test]
        public void GetByKeyAsync_NotExistingKey_ThrowNotFoundException()
        {
            _mockGameService.Setup(x => x.GetByKeyAsync(It.IsAny<string>()))
                .Throws(() => new NotFoundException());

            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);

            Assert.ThrowsAsync<NotFoundException>(() => gameController.GetGameByKeyAsync(""));

            _mockGameService.Verify(x => x.GetByKeyAsync(""), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidGameModel_ExecOnce()
        {
            _mockGameService.Setup(x => x.CreateAsync(It.IsAny<GameModel>())).ReturnsAsync(() => new GameModel());
            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);
            GameModel game = new GameModel() { PublisherId = userId };

            await gameController.CreateAsync(game);

            _mockGameService.Verify(x => x.CreateAsync(game), Times.Once);
        }

        [Test]
        public void CreateAsync_NotUniqueKey_ThrowModelException()
        {
            _mockGameService.Setup(x => x.CreateAsync(It.IsAny<GameModel>()))
                .Throws(() => new ModelException());

            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);
            GameModel game = new GameModel();

            Assert.ThrowsAsync<ModelException>(() => gameController.CreateAsync(game));

            _mockGameService.Verify(x => x.CreateAsync(game), Times.Once);
        }

        [Test]
        public async Task EditAsync_ValidGameModel_ExecOnce()
        {
            _mockGameService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new GameModel() { PublisherId = Guid.Empty });
            _mockGameService.Setup(x => x.EditAsync(It.IsAny<GameModel>())).ReturnsAsync(() => new GameModel());
            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);
            GameModel game = new GameModel() { PublisherId = Guid.Empty };

            await gameController.EditAsync(game);

            _mockGameService.Verify(x => x.EditAsync(It.IsAny<GameModel>()), Times.Once);
        }

        [Test]
        public async Task EditAsync_InvalidPublisher_ExecNever()
        {
            _mockGameService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new GameModel() { PublisherId = Guid.NewGuid() });

            _mockGameService.Setup(x => x.EditAsync(It.IsAny<GameModel>()));

            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);
            GameModel game = new GameModel() { PublisherId = Guid.NewGuid() };

            await gameController.EditAsync(game);

            _mockGameService.Verify(x => x.EditAsync(It.IsAny<GameModel>()), Times.Never);
        }

        [Test]
        public void EditAsync_NotUniqueKey_ThrowModelException()
        {
            _mockGameService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new GameModel() { PublisherId = Guid.Empty });

            _mockGameService.Setup(x => x.EditAsync(It.IsAny<GameModel>()))
                .Throws(() => new ModelException());

            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);
            GameModel game = new GameModel() { PublisherId = Guid.Empty };

            Assert.ThrowsAsync<ModelException>(() => gameController.EditAsync(game));

            _mockGameService.Verify(x => x.EditAsync(It.IsAny<GameModel>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ValidGameModel_ExecOnce()
        {
            _mockGameService.Setup(x => x.DeleteAsync(It.IsAny<Guid>()));
            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);
            Guid gameId = Guid.NewGuid();

            await gameController.DeleteAsync(gameId);

            _mockGameService.Verify(x => x.DeleteAsync(gameId), Times.Once);
        }

        [Test]
        public void DeleteAsync_NotUniqueKey_ThrowModelException()
        {
            _mockGameService.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .Throws(() => new ModelException());

            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);
            Guid gameId = Guid.NewGuid();

            Assert.ThrowsAsync<ModelException>(() => gameController.DeleteAsync(gameId));

            _mockGameService.Verify(x => x.DeleteAsync(gameId), Times.Once);
        }

        [Test]
        public void Download_ValidGameKey_ExecOnce()
        {
            _mockEnvironment.Setup(x => x.ContentRootPath).Returns("example");
            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);

            var actual = gameController.Download("");
            Assert.NotNull(actual);
        }

        [Test]
        public async Task GetGamesNumber_ExecOnce()
        {
            _mockGameService.Setup(x => x.GetGameNumberAsync());
            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);
            Guid gameId = Guid.NewGuid();

            await gameController.GetGamesNumber();

            _mockGameService.Verify(x => x.GetGameNumberAsync(), Times.Once);
        }

        [Test]
        public async Task EditGameViewNumber_ExecOnce()
        {
            _mockGameService.Setup(x => x.EditViewNumberAsync(It.IsAny<string>()));
            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);
            Guid gameId = Guid.NewGuid();

            await gameController.EditViewNumberAsync("key");

            _mockGameService.Verify(x => x.EditViewNumberAsync("key"), Times.Once);
        }

        [Test]
        public async Task GetAllAsyncByFilter_FilterExist_ExecOnce()
        {
            _mockGameService.Setup(x => x.GetByFilter(It.IsAny<string>()));
            GameController gameController = new GameController(_mockEnvironment.Object, _mockGameService.Object, _mockUserClaims.Object);
            Guid gameId = Guid.NewGuid();

            await gameController.GetAllAsync("");

            _mockGameService.Verify(x => x.GetByFilter(It.IsAny<string>()), Times.Once);
        }
    }
}