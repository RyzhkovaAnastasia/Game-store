using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.API.UnitTests
{
    public class ShipperControllerTests
    {
        private Mock<IShipperService> _mockShipperService;

        [SetUp]
        public void Setup()
        {
            _mockShipperService = new Mock<IShipperService>();
        }

        [Test]
        public async Task GetAllAsyncShippersAsync_ExecOnce()
        {
            _mockShipperService.Setup(x => x.GetAllAsync()).ReturnsAsync(() => new List<ShipperModel>());
            ShipperController shipperController = new ShipperController(_mockShipperService.Object);

            var actual = await shipperController.GetAllAsync();

            _mockShipperService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ValidKey_ExecOnce()
        {
            _mockShipperService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new ShipperModel());
            ShipperController shipperController = new ShipperController(_mockShipperService.Object);
            Guid shipperId = Guid.NewGuid();

            await shipperController.GetByIdAsync(shipperId);

            _mockShipperService.Verify(x => x.GetByIdAsync(shipperId), Times.Once);
        }
    }
}
