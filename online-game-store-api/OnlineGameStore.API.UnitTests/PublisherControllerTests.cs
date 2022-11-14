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
    public class PublisherControllerTests
    {
        private Mock<IPublisherService> _mockPublisherService;
        private Mock<UserClaims> _mockUserClaims;

        [SetUp]
        public void Setup()
        {
            _mockPublisherService = new Mock<IPublisherService>();
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext.Request.Cookies["guestId"]).Returns(Guid.NewGuid().ToString());
            accessor.Setup(x => x.HttpContext.User).Returns(() => new ClaimsPrincipal());
            _mockUserClaims = new Mock<UserClaims>(accessor.Object);
        }

        [Test]
        public async Task GetAllAsyncPublishersAsync_ExecOnce()
        {
            _mockPublisherService.Setup(x => x.GetAllAsync()).ReturnsAsync(() => new List<PublisherModel>());
            PublisherController publisherController = new PublisherController(_mockPublisherService.Object, _mockUserClaims.Object);

            var actual = await publisherController.GetAllAsync();

            _mockPublisherService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetByKeyAsync_ValidKey_ExecOnce()
        {
            _mockPublisherService.Setup(x => x.GetByCompanyNameAsync(It.IsAny<string>())).ReturnsAsync(() => new PublisherModel());
            PublisherController publisherController = new PublisherController(_mockPublisherService.Object, _mockUserClaims.Object);

            await publisherController.GetByCompanyNameAsync("");

            _mockPublisherService.Verify(x => x.GetByCompanyNameAsync(""), Times.Once);
        }

        [Test]
        public void GetByKeyAsync_NotExistingKey_ThrowNotFoundException()
        {
            _mockPublisherService.Setup(x => x.GetByCompanyNameAsync(It.IsAny<string>()))
                .Throws(() => new NotFoundException());

            PublisherController publisherController = new PublisherController(_mockPublisherService.Object, _mockUserClaims.Object);

            Assert.ThrowsAsync<NotFoundException>(() => publisherController.GetByCompanyNameAsync(""));

            _mockPublisherService.Verify(x => x.GetByCompanyNameAsync(""), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidPublisherModel_ExecOnce()
        {
            _mockPublisherService.Setup(x => x.CreateAsync(It.IsAny<PublisherModel>())).ReturnsAsync(() => new PublisherModel());
            PublisherController publisherController = new PublisherController(_mockPublisherService.Object, _mockUserClaims.Object);
            PublisherModel publisher = new PublisherModel();

            await publisherController.CreateAsync(publisher);

            _mockPublisherService.Verify(x => x.CreateAsync(publisher), Times.Once);
        }

        [Test]
        public void CreateAsync_NotUniqueKey_ThrowModelException()
        {
            _mockPublisherService.Setup(x => x.CreateAsync(It.IsAny<PublisherModel>()))
                .Throws(() => new ModelException());

            PublisherController publisherController = new PublisherController(_mockPublisherService.Object, _mockUserClaims.Object);
            PublisherModel publisher = new PublisherModel();

            Assert.ThrowsAsync<ModelException>(() => publisherController.CreateAsync(publisher));

            _mockPublisherService.Verify(x => x.CreateAsync(publisher), Times.Once);
        }

        [Test]
        public async Task EditAsync_ValidPublisherModel_ExecOnce()
        {
            _mockPublisherService.Setup(x => x.EditAsync(It.IsAny<PublisherModel>())).ReturnsAsync(() => new PublisherModel());
            PublisherController publisherController = new PublisherController(_mockPublisherService.Object, _mockUserClaims.Object);
            PublisherModel publisher = new PublisherModel();

            await publisherController.EditAsync(publisher);

            _mockPublisherService.Verify(x => x.EditAsync(publisher), Times.Once);
        }

        [Test]
        public void EditAsync_NotUniqueKey_ThrowModelException()
        {
            _mockPublisherService.Setup(x => x.EditAsync(It.IsAny<PublisherModel>()))
                .Throws(() => new ModelException());

            PublisherController publisherController = new PublisherController(_mockPublisherService.Object, _mockUserClaims.Object);
            PublisherModel publisher = new PublisherModel();

            Assert.ThrowsAsync<ModelException>(() => publisherController.EditAsync(publisher));

            _mockPublisherService.Verify(x => x.EditAsync(publisher), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ValidPublisherModel_ExecOnce()
        {
            _mockPublisherService.Setup(x => x.DeleteAsync(It.IsAny<Guid>()));
            PublisherController publisherController = new PublisherController(_mockPublisherService.Object, _mockUserClaims.Object);
            Guid publisherId = Guid.NewGuid();

            await publisherController.DeleteAsync(publisherId);

            _mockPublisherService.Verify(x => x.DeleteAsync(publisherId), Times.Once);
        }

        [Test]
        public void DeleteAsync_NotUniqueKey_ThrowModelException()
        {
            _mockPublisherService.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .Throws(() => new ModelException());

            PublisherController publisherController = new PublisherController(_mockPublisherService.Object, _mockUserClaims.Object);
            Guid publisherId = Guid.NewGuid();

            Assert.ThrowsAsync<ModelException>(() => publisherController.DeleteAsync(publisherId));

            _mockPublisherService.Verify(x => x.DeleteAsync(publisherId), Times.Once);
        }
    }
}

