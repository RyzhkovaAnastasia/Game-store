using AutoMapper;
using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.Configurations.MapperConfigurations;
using OnlineGameStore.BLL.CustomExceptions;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.UnitTests
{
    public class OrderServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<Order>> _mockOrderRepository;
        private readonly IMapper _mapper;
        public OrderServiceTests()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mc => new MappingProfile(mc));
            _mapper = mapperConfiguration.CreateMapper();
        }

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockUnitOfWork.Setup(x => x.OrderRepository).Returns(_mockOrderRepository.Object);
        }

        [Test]
        public async Task GetAll_ReturnOrderList()
        {
            _mockOrderRepository
                .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(() => new List<Order>());

            OrderService orderService = new OrderService(_mockUnitOfWork.Object, _mapper);

            var orders = await orderService.GetAllAsync(DateTime.MinValue, DateTime.MaxValue);

            Assert.IsNotNull(orders);
            _mockUnitOfWork.Verify(x => x.OrderRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderRepository.GetAllAsync(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnOrder()
        {
            _mockOrderRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(() => new Order());

            OrderService orderService = new OrderService(_mockUnitOfWork.Object, _mapper);


            OrderModel order = await orderService.GetByIdAsync(Guid.NewGuid());


            Assert.IsNotNull(order);
            _mockUnitOfWork.Verify(x => x.OrderRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderRepository.FindAsync(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once);
        }

        [Test]
        public void GetByIdAsync_NotExistingId_ThrowNotFoundException()
        {

            _mockOrderRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(() => null);

            OrderService orderService = new OrderService(_mockUnitOfWork.Object, _mapper);



            Assert.ThrowsAsync<NotFoundException>(() => orderService.GetByIdAsync(Guid.NewGuid()));
            _mockUnitOfWork.Verify(x => x.OrderRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderRepository.FindAsync(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once);
        }


        [Test]
        public async Task EditAsync_ValidOrder_ReturnNewOrderModel()
        {

            _mockOrderRepository
                .Setup(x => x.EditAsync(It.IsAny<Order>()))
                .ReturnsAsync(() => new Order());

            OrderService orderService = new OrderService(_mockUnitOfWork.Object, _mapper);
            OrderModel editedOrderModel = new OrderModel() { Items = new List<OrderDetailModel>() };


            OrderModel orders = await orderService.EditAsync(editedOrderModel);


            Assert.IsNotNull(orders);
            _mockUnitOfWork.Verify(x => x.OrderRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderRepository.EditAsync(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        public void EditAsync_InvalidOrderId_ThrowException()
        {

            _mockOrderRepository
                .Setup(x => x.EditAsync(It.IsAny<Order>()))
                .ReturnsAsync(() => null);

            OrderService orderService = new OrderService(_mockUnitOfWork.Object, _mapper);
            OrderModel editedOrderModel = new OrderModel();



            Assert.ThrowsAsync<NotFoundException>(() => orderService.EditAsync(editedOrderModel));
            _mockUnitOfWork.Verify(x => x.OrderRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderRepository.EditAsync(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        public void Editsync_InvalidOrder_ThrowException()
        {

            _mockOrderRepository
                .Setup(x => x.EditAsync(It.IsAny<Order>()))
                .Throws(new Exception());

            OrderService orderService = new OrderService(_mockUnitOfWork.Object, _mapper);
            OrderModel updatedOrderModel = new OrderModel();


            Assert.ThrowsAsync<Exception>(() => orderService.EditAsync(updatedOrderModel));

            _mockUnitOfWork.Verify(x => x.OrderRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderRepository.EditAsync(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidOrder_ReturnNewOrderModel()
        {

            _mockOrderRepository
                .Setup(x => x.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync(() => new Order());

            OrderService orderService = new OrderService(_mockUnitOfWork.Object, _mapper);
            OrderModel newOrderModel = new OrderModel();


            OrderModel orders = await orderService.CreateAsync(newOrderModel);


            Assert.IsNotNull(orders);
            _mockUnitOfWork.Verify(x => x.OrderRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderRepository.CreateAsync(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        public void CreateAsync_InvalidOrder_ThrowException()
        {

            _mockOrderRepository
                .Setup(x => x.CreateAsync(It.IsAny<Order>()))
                .Throws(new Exception());

            OrderService orderService = new OrderService(_mockUnitOfWork.Object, _mapper);
            OrderModel newOrderModel = new OrderModel();


            Assert.ThrowsAsync<Exception>(() => orderService.CreateAsync(newOrderModel));

            _mockUnitOfWork.Verify(x => x.OrderRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderRepository.CreateAsync(It.IsAny<Order>()), Times.Once);
        }


        [Test]
        public async Task DeleteAsync_ValidOrder_DeleteOrder()
        {

            _mockOrderRepository
                .Setup(x => x.SoftDeleteAsync(It.IsAny<Guid>()));

            OrderService orderService = new OrderService(_mockUnitOfWork.Object, _mapper);


            await orderService.DeleteAsync(Guid.NewGuid());


            _mockUnitOfWork.Verify(x => x.OrderRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderRepository.SoftDeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void DeleteAsync_InvalidOrder_ThrowException()
        {

            _mockOrderRepository
                .Setup(x => x.SoftDeleteAsync(It.IsAny<Guid>()))
                .Throws(new Exception());

            OrderService orderService = new OrderService(_mockUnitOfWork.Object, _mapper);


            Assert.ThrowsAsync<Exception>(() => orderService.DeleteAsync(Guid.NewGuid()));

            _mockUnitOfWork.Verify(x => x.OrderRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.OrderRepository.SoftDeleteAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
