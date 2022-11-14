using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
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
    public class OrderControllerTests
    {
        private Mock<IOrderService> _mockOrderService;
        private Mock<UserClaims> _mockUserClaims;

        [SetUp]
        public void Setup()
        {
            _mockOrderService = new Mock<IOrderService>();

            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext.Request.Cookies["guestId"]).Returns(Guid.NewGuid().ToString());
            accessor.Setup(x => x.HttpContext.User).Returns(() => new ClaimsPrincipal());
            _mockUserClaims = new Mock<UserClaims>(accessor.Object);
        }

        [Test]
        public async Task GetActiveOrder_ExecOnce()
        {
            Guid orderId = Guid.NewGuid();
            _mockOrderService.Setup(x => x.GetLastOrderByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new OrderModel());
            OrderController orderController = new OrderController(_mockOrderService.Object, _mockUserClaims.Object);

            var actual = await orderController.GetActiveOrder();

            _mockOrderService.Verify(x => x.GetLastOrderByUserIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task RemoveItems_ExecOnce()
        {
            _mockOrderService.Setup(x => x.GetLastOrderByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new OrderModel());

            _mockOrderService.Setup(x => x.EditAsync(It.IsAny<OrderModel>()));
            OrderController OrderDetailController = new OrderController(_mockOrderService.Object, _mockUserClaims.Object);
            OrderModel order = new OrderModel();

            var actual = await OrderDetailController.EditOrder(order);

            _mockOrderService.Verify(x => x.EditAsync(order), Times.Once);
        }

        [Test]
        public async Task GetOrdersByDates_ExecOnce()
        {
            _mockOrderService.Setup(x => x.GetAllAsync(DateTime.MinValue, DateTime.MaxValue))
                .ReturnsAsync(() => new List<OrderModel>());

            OrderController orderController = new OrderController(_mockOrderService.Object, _mockUserClaims.Object);

            await orderController.GetOrders(new DateQueryModel() { StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue });

            _mockOrderService.Verify(x => x.GetAllAsync(DateTime.MinValue, DateTime.MaxValue), Times.Once);
        }
    }
}
