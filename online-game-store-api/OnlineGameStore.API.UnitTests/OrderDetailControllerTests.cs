using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Controllers;
using OnlineGameStore.Extensions;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineGameStore.API.UnitTests
{
    public class OrderDetailControllerTests
    {
        private Mock<IOrderDetailService> _mockOrderDetailService;
        private Mock<UserClaims> _mockUserClaims;

        [SetUp]
        public void Setup()
        {
            _mockOrderDetailService = new Mock<IOrderDetailService>();
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext.Request.Cookies["guestId"]).Returns(Guid.NewGuid().ToString());
            accessor.Setup(x => x.HttpContext.User).Returns(() => new ClaimsPrincipal());
            _mockUserClaims = new Mock<UserClaims>(accessor.Object);
        }

        [Test]
        public async Task AddItems_ExecOnce()
        {
            _mockOrderDetailService.Setup(x => x.AddToBasketAsync(It.IsAny<OrderDetailModel>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => new OrderDetailModel());

            var OrderDetailController = new OrderDetailController(_mockOrderDetailService.Object, _mockUserClaims.Object);
            OrderDetailModel orderDetail = new OrderDetailModel();

            IActionResult actual = await OrderDetailController.AddBasketItem(orderDetail);

            _mockOrderDetailService.Verify(x => x.AddToBasketAsync(It.IsAny<OrderDetailModel>(), It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task RemoveItems_ExecOnce()
        {
            _mockOrderDetailService.Setup(x => x.RemoveFromBasketAsync(It.IsAny<Guid>()));

            var OrderDetailController = new OrderDetailController(_mockOrderDetailService.Object, _mockUserClaims.Object);
            OrderDetailModel orderDetail = new OrderDetailModel();

            IActionResult actual = await OrderDetailController.DeleteBasketItem(Guid.NewGuid());

            _mockOrderDetailService.Verify(x => x.RemoveFromBasketAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
