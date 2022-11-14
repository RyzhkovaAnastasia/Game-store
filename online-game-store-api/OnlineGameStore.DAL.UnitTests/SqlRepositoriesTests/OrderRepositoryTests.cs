using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NUnit.Framework;
using OnlineGameStore.DAL.Context;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.UnitTests.SqlRepositoriesTests
{
    internal class OrderRepositoryTests
    {
        private readonly IGameStoreDBContext _context;
        private readonly ISQLRepository<Order> _orderRepository;
        private TestData _contextData;

        public OrderRepositoryTests()
        {
            DbContextOptionsBuilder<GameStoreDBContext> options = new DbContextOptionsBuilder<GameStoreDBContext>();
            options.UseInMemoryDatabase("OnlineGameStore");
            _context = new GameStoreDBContext(options.Options);
            _orderRepository = new SQLRepository<Order>(_context);
        }

        [SetUp]
        public void Setup()
        {
            _contextData = new TestData(_context);
        }

        [TearDown]
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task CreateAsync_ValidOrderEntity_ReturnOrderEntity()
        {

            Order expected = new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };


            Order actual = await _orderRepository.CreateAsync(expected);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateAsync_InsertExistingOrderEntity_ThrowException()
        {

            Order existingEntity = _contextData.Orders[0];

            Assert.CatchAsync<ArgumentException>(() => _orderRepository.CreateAsync(existingEntity));
        }

        [Test]
        public async Task CreateAsync_NullOrderEntity_ReturnNull()
        {

            Order expected = null;


            Order actual = await _orderRepository.CreateAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_ValidOrderEntity_ReturnOrderEntity()
        {

            Order expected = _contextData.Orders[0];
            expected.Items = new List<OrderDetail>();


            Order actual = await _orderRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_NullOrderEntity_ReturnNull()
        {

            Order expected = null;


            Order actual = await _orderRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_OrderEntityId_ReturnOrderEntity()
        {

            Order expected = _contextData.Orders[0];


            Order actual = await _orderRepository.GetByIdAsync(expected.Id);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_NotExistingOrderId_ReturnNull()
        {

            Order expected = null;
            Guid notExistingId = Guid.NewGuid();

            Order actual = await _orderRepository.GetByIdAsync(notExistingId);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_OrderEntityId_ReturnOrderEntity()
        {

            Order expected = _contextData.Orders[0];


            Order actual = await _orderRepository.FindAsync(order => order.Id == expected.Id);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NotExistingOrderId_ReturnNull()
        {

            Order expected = null;
            Guid notExistingId = Guid.NewGuid();

            Order actual = await _orderRepository.FindAsync(order => order.Id == notExistingId);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NullPredicate_ReturnNull()
        {

            Order expected = null;


            Order actual = await _orderRepository.FindAsync(null);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetWhere_ValidPredicate_ReturnOrderList()
        {

            IEnumerable<Order> expected = _contextData.Orders;


            IEnumerable<Order> actual = await _orderRepository.GetAllAsync(order => order.Id != null);


            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetWhere_NullPredicate_ReturnAll()
        {

            IEnumerable<Order> expected = _contextData.Orders;


            IEnumerable<Order> actual = await _orderRepository.GetAllAsync(null);


            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [Test]
        public async Task DeleteAsync_ValidId_UpdateIsDeleted()
        {

            Order orderToDelete = _contextData.Orders[^1];
            int expectedOrderCount = _contextData.Orders.Count - 1;


            await _orderRepository.DeleteAsync(orderToDelete.Id);


            int actualOrderCount = await _context.Orders.CountAsync();


            Assert.AreEqual(expectedOrderCount, actualOrderCount);
        }

        [Test]
        public async Task GetAllAsync_ReturnAllNotDeletedOrders()
        {

            List<Order> expected = _contextData.Orders;


            IEnumerable<Order> actual = await _orderRepository.GetAllAsync();


            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}
