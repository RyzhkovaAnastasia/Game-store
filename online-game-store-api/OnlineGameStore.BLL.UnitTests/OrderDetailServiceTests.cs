using AutoMapper;
using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.Configurations.MapperConfigurations;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.UnitTests
{
    public class OrderDetailServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<OrderDetail>> _mockOrderDetailRepository;
        private readonly IMapper _mapper;
        public OrderDetailServiceTests()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mc => new MappingProfile(mc));
            _mapper = mapperConfiguration.CreateMapper();
        }

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderDetailRepository = new Mock<IRepository<OrderDetail>>();
            _mockUnitOfWork.Setup(x => x.OrderDetailRepository).Returns(_mockOrderDetailRepository.Object);
        }

        [Test]
        public async Task AddToBasketAsync_ValidOrderDetail_ReturnNewOrderDetailModel()
        {
            _mockOrderDetailRepository
                .Setup(x => x.CreateAsync(It.IsAny<OrderDetail>()))
                .ReturnsAsync(() => new OrderDetail());

            var _mockOrderService = new Mock<IOrderService>();
            var _mockGameRepository = new Mock<IRepository<Game>>();

            _mockOrderService
               .Setup(x => x.GetLastOrderByUserIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(() => new OrderModel());

            _mockUnitOfWork.Setup(x => x.GameRepository).Returns(_mockGameRepository.Object);

            var orderDetailService = new OrderDetailService(_mockUnitOfWork.Object, _mockOrderService.Object, _mapper);
            OrderDetailModel newOrderDetailModel = new OrderDetailModel();


            await orderDetailService.AddToBasketAsync(newOrderDetailModel, Guid.NewGuid());

            _mockUnitOfWork.Verify(x => x.OrderDetailRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderDetailRepository.CreateAsync(It.IsAny<OrderDetail>()), Times.Once);
        }

        [Test]
        public void AddToBasketAsync_InvalidOrderDetail_ThrowException()
        {
            _mockOrderDetailRepository
                .Setup(x => x.CreateAsync(It.IsAny<OrderDetail>()))
                .Throws(new Exception());

            var _mockOrderService = new Mock<IOrderService>();
            var _mockGameRepository = new Mock<IRepository<Game>>();

            _mockOrderService
               .Setup(x => x.GetLastOrderByUserIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(() => new OrderModel());

            _mockUnitOfWork.Setup(x => x.GameRepository).Returns(_mockGameRepository.Object);

            var orderDetailService = new OrderDetailService(_mockUnitOfWork.Object, _mockOrderService.Object, _mapper);
            OrderDetailModel newOrderDetailModel = new OrderDetailModel();


            Assert.ThrowsAsync<Exception>(() => orderDetailService.AddToBasketAsync(newOrderDetailModel, Guid.NewGuid()));

            _mockUnitOfWork.Verify(x => x.OrderDetailRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderDetailRepository.CreateAsync(It.IsAny<OrderDetail>()), Times.Once);
        }


        [Test]
        public async Task DeleteAsync_ValidOrderDetail_DeleteOrderDetail()
        {

            _mockOrderDetailRepository
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()));

            OrderDetailService orderDetailService = new OrderDetailService(_mockUnitOfWork.Object, null, _mapper);


            await orderDetailService.RemoveFromBasketAsync(Guid.NewGuid());


            _mockUnitOfWork.Verify(x => x.OrderDetailRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderDetailRepository.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void DeleteAsync_InvalidOrderDetail_ThrowException()
        {

            _mockOrderDetailRepository
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .Throws(new Exception());

            OrderDetailService orderDetailService = new OrderDetailService(_mockUnitOfWork.Object, null, _mapper);


            Assert.ThrowsAsync<Exception>(() => orderDetailService.RemoveFromBasketAsync(Guid.NewGuid()));

            _mockUnitOfWork.Verify(x => x.OrderDetailRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderDetailRepository.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
