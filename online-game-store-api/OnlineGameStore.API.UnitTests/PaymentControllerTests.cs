using Microsoft.AspNetCore.Hosting;
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
    public class PaymentControllerTests
    {
        private Mock<IPaymentMethodService> _mockPaymentMethodService;
        private Mock<IWebHostEnvironment> _mockWebHostEnv;
        private Mock<UserClaims> _mockUserClaims;

        [SetUp]
        public void Setup()
        {
            _mockPaymentMethodService = new Mock<IPaymentMethodService>();

            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext.Request.Cookies["guestId"]).Returns(Guid.NewGuid().ToString());
            accessor.Setup(x => x.HttpContext.User).Returns(() => new ClaimsPrincipal());
            _mockUserClaims = new Mock<UserClaims>(accessor.Object);

            _mockWebHostEnv = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetAllAsync_ExecOnce()
        {
            _mockPaymentMethodService.Setup(x => x.GetAllAsync()).ReturnsAsync(() => new List<PaymentMethodModel>());

            var paymentController = new PaymentController(_mockWebHostEnv.Object, _mockPaymentMethodService.Object, _mockUserClaims.Object);

            await paymentController.GetAllAsync();

            _mockPaymentMethodService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task BankPaymentAsync_ValidPath_ExecOnce()
        {
            _mockWebHostEnv.Setup(x => x.ContentRootPath).Returns("example");
            _mockPaymentMethodService.Setup(x => x.PayAsync(It.IsAny<Guid>(), It.IsAny<object>()));

            var paymentController = new PaymentController(_mockWebHostEnv.Object, _mockPaymentMethodService.Object, _mockUserClaims.Object);

            await paymentController.BankPaymentAsync();

            _mockPaymentMethodService.Verify(x => x.PayAsync(It.IsAny<Guid>(), It.IsAny<object>()), Times.Once);
        }

        [Test]
        public async Task IBoxPaymentAsync_ExecOnce()
        {
            Guid customerId = Guid.NewGuid();

            _mockPaymentMethodService
                .Setup(x => x.PayAsync(It.IsAny<Guid>(), It.IsAny<object>()));

            var paymentController = new PaymentController(_mockWebHostEnv.Object, _mockPaymentMethodService.Object, _mockUserClaims.Object);
            IBoxModel iboxModel = new IBoxModel();

            await paymentController.IBoxPaymentAsync(iboxModel);

            _mockPaymentMethodService.Verify(x => x.PayAsync(It.IsAny<Guid>(), It.IsAny<object>()), Times.Once);
        }

        [Test]
        public async Task VisaPaymentAsync_ExecOnce()
        {
            Guid customerId = Guid.NewGuid();

            _mockPaymentMethodService
                .Setup(x => x.PayAsync(It.IsAny<Guid>(), It.IsAny<object>()));

            var paymentController = new PaymentController(_mockWebHostEnv.Object, _mockPaymentMethodService.Object, _mockUserClaims.Object);
            VisaModel visaModel = new VisaModel();

            await paymentController.VisaPaymentAsync(visaModel);

            _mockPaymentMethodService.Verify(x => x.PayAsync(It.IsAny<Guid>(), It.IsAny<object>()), Times.Once);
        }

        [Test]
        public async Task StartPaymentTimeout_ExecOnce()
        {
            Guid customerId = Guid.NewGuid();

            var paymentController = new PaymentController(_mockWebHostEnv.Object, _mockPaymentMethodService.Object, _mockUserClaims.Object);

            await paymentController.StartPaymentTimeout(15);

            _mockPaymentMethodService.Verify(x => x.StartTimeout(It.IsAny<Guid>(), 15), Times.Once);
        }
    }
}
