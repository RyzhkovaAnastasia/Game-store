using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.CustomExceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.API.UnitTests
{
    public class PlatformTypeControllerTests
    {
        private Mock<IPlatformTypeService> _mockPlatformTypeService;

        [SetUp]
        public void Setup()
        {
            _mockPlatformTypeService = new Mock<IPlatformTypeService>();
        }

        [Test]
        public async Task GetAllAsyncPlatformTypesAsync_ExecOnce()
        {
            _mockPlatformTypeService.Setup(x => x.GetAllAsync()).ReturnsAsync(() => new List<PlatformTypeModel>());
            PlatformTypeController platformTypeController = new PlatformTypeController(_mockPlatformTypeService.Object);

            var actual = await platformTypeController.GetAllAsync();

            _mockPlatformTypeService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetByKeyAsync_ValidKey_ExecOnce()
        {
            _mockPlatformTypeService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new PlatformTypeModel());
            PlatformTypeController platformTypeController = new PlatformTypeController(_mockPlatformTypeService.Object);
            Guid platformId = Guid.NewGuid();

            await platformTypeController.GetByIdAsync(platformId);

            _mockPlatformTypeService.Verify(x => x.GetByIdAsync(platformId), Times.Once);
        }

        [Test]
        public void GetByKeyAsync_NotExistingKey_ThrowNotFoundException()
        {
            _mockPlatformTypeService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Throws(() => new NotFoundException());

            PlatformTypeController platformTypeController = new PlatformTypeController(_mockPlatformTypeService.Object);
            Guid platformId = Guid.NewGuid();

            Assert.ThrowsAsync<NotFoundException>(() => platformTypeController.GetByIdAsync(platformId));

            _mockPlatformTypeService.Verify(x => x.GetByIdAsync(platformId), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidPlatformTypeModel_ExecOnce()
        {
            _mockPlatformTypeService.Setup(x => x.CreateAsync(It.IsAny<PlatformTypeModel>())).ReturnsAsync(() => new PlatformTypeModel());
            PlatformTypeController platformTypeController = new PlatformTypeController(_mockPlatformTypeService.Object);
            PlatformTypeModel platformType = new PlatformTypeModel();

            await platformTypeController.CreateAsync(platformType);

            _mockPlatformTypeService.Verify(x => x.CreateAsync(platformType), Times.Once);
        }

        [Test]
        public void CreateAsync_NotUniqueKey_ThrowModelException()
        {
            _mockPlatformTypeService.Setup(x => x.CreateAsync(It.IsAny<PlatformTypeModel>()))
                .Throws(() => new ModelException());

            PlatformTypeController platformTypeController = new PlatformTypeController(_mockPlatformTypeService.Object);
            PlatformTypeModel platformType = new PlatformTypeModel();

            Assert.ThrowsAsync<ModelException>(() => platformTypeController.CreateAsync(platformType));

            _mockPlatformTypeService.Verify(x => x.CreateAsync(platformType), Times.Once);
        }

        [Test]
        public async Task EditAsync_ValidPlatformTypeModel_ExecOnce()
        {
            _mockPlatformTypeService.Setup(x => x.EditAsync(It.IsAny<PlatformTypeModel>())).ReturnsAsync(() => new PlatformTypeModel());
            PlatformTypeController platformTypeController = new PlatformTypeController(_mockPlatformTypeService.Object);
            PlatformTypeModel platformType = new PlatformTypeModel();

            await platformTypeController.EditAsync(platformType);

            _mockPlatformTypeService.Verify(x => x.EditAsync(platformType), Times.Once);
        }

        [Test]
        public void EditAsync_NotUniqueKey_ThrowModelException()
        {
            _mockPlatformTypeService.Setup(x => x.EditAsync(It.IsAny<PlatformTypeModel>()))
                .Throws(() => new ModelException());

            PlatformTypeController platformTypeController = new PlatformTypeController(_mockPlatformTypeService.Object);
            PlatformTypeModel platformType = new PlatformTypeModel();

            Assert.ThrowsAsync<ModelException>(() => platformTypeController.EditAsync(platformType));

            _mockPlatformTypeService.Verify(x => x.EditAsync(platformType), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ValidPlatformTypeModel_ExecOnce()
        {
            _mockPlatformTypeService.Setup(x => x.DeleteAsync(It.IsAny<Guid>()));
            PlatformTypeController platformTypeController = new PlatformTypeController(_mockPlatformTypeService.Object);
            Guid platformTypeId = Guid.NewGuid();

            await platformTypeController.DeleteAsync(platformTypeId);

            _mockPlatformTypeService.Verify(x => x.DeleteAsync(platformTypeId), Times.Once);
        }

        [Test]
        public void DeleteAsync_NotUniqueKey_ThrowModelException()
        {
            _mockPlatformTypeService.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .Throws(() => new ModelException());

            PlatformTypeController platformTypeController = new PlatformTypeController(_mockPlatformTypeService.Object);
            Guid platformTypeId = Guid.NewGuid();

            Assert.ThrowsAsync<ModelException>(() => platformTypeController.DeleteAsync(platformTypeId));

            _mockPlatformTypeService.Verify(x => x.DeleteAsync(platformTypeId), Times.Once);
        }
    }
}