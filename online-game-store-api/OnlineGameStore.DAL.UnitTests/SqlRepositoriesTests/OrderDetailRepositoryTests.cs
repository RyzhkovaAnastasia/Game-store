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
    internal class OrderDetailRepositoryTests
    {
        private readonly IGameStoreDBContext _context;
        private readonly ISQLRepository<OrderDetail> _orderDetailRepository;
        private TestData _contextData;

        public OrderDetailRepositoryTests()
        {
            DbContextOptionsBuilder<GameStoreDBContext> options = new DbContextOptionsBuilder<GameStoreDBContext>();
            options.UseInMemoryDatabase("OnlineGameStore");
            _context = new GameStoreDBContext(options.Options);
            _orderDetailRepository = new SQLRepository<OrderDetail>(_context);
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
        public async Task CreateAsync_ValidOrderDetailEntity_ReturnOrderDetailEntity()
        {

            OrderDetail expected = new OrderDetail
            {
                Id = Guid.NewGuid(),
                Price = 100,
                Discount = 0,
                OrderId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Quantity = 1
            };


            OrderDetail actual = await _orderDetailRepository.CreateAsync(expected);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateAsync_InsertExistingOrderDetailEntity_ThrowException()
        {

            OrderDetail existingEntity = _contextData.OrderDetails[0];
            Assert.CatchAsync<ArgumentException>(() => _orderDetailRepository.CreateAsync(existingEntity));
        }

        [Test]
        public async Task CreateAsync_NullOrderDetailEntity_ReturnNull()
        {

            OrderDetail expected = null;


            OrderDetail actual = await _orderDetailRepository.CreateAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_ValidOrderDetailEntity_ReturnOrderDetailEntity()
        {

            OrderDetail expected = _contextData.OrderDetails[0];
            expected.Discount = 50;


            OrderDetail actual = await _orderDetailRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_NullOrderDetailEntity_ReturnNull()
        {

            OrderDetail expected = null;


            OrderDetail actual = await _orderDetailRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_OrderDetailEntityId_ReturnOrderDetailEntity()
        {

            OrderDetail expected = _contextData.OrderDetails[0];


            OrderDetail actual = await _orderDetailRepository.GetByIdAsync(expected.Id);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_NotExistingOrderDetailId_ReturnNull()
        {

            OrderDetail expected = null;
            Guid notExistingId = Guid.NewGuid();

            OrderDetail actual = await _orderDetailRepository.GetByIdAsync(notExistingId);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_OrderDetailEntityId_ReturnOrderDetailEntity()
        {

            OrderDetail expected = _contextData.OrderDetails[0];


            OrderDetail actual = await _orderDetailRepository.FindAsync(orderDetail => orderDetail.Id == expected.Id);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NotExistingOrderDetailId_ReturnNull()
        {

            OrderDetail expected = null;
            Guid notExistingId = Guid.NewGuid();

            OrderDetail actual = await _orderDetailRepository.FindAsync(orderDetail => orderDetail.Id == notExistingId);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NullPredicate_ReturnNull()
        {

            OrderDetail expected = null;


            OrderDetail actual = await _orderDetailRepository.FindAsync(null);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetWhere_ValidPredicate_ReturnOrderDetailList()
        {

            IEnumerable<OrderDetail> expected = _contextData.OrderDetails;


            IEnumerable<OrderDetail> actual = await _orderDetailRepository.GetAllAsync(orderDetail => orderDetail.Id != null);


            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetWhere_NullPredicate_ReturnAll()
        {

            IEnumerable<OrderDetail> expected = _contextData.OrderDetails;


            IEnumerable<OrderDetail> actual = await _orderDetailRepository.GetAllAsync(null);


            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [Test]
        public async Task DeleteAsync_ValidId_UpdateIsDeleted()
        {

            OrderDetail orderDetailToDelete = _contextData.OrderDetails[^1];
            int expectedOrderDetailCount = _contextData.OrderDetails.Count - 1;


            await _orderDetailRepository.DeleteAsync(orderDetailToDelete.Id);


            int actualOrderDetailCount = await _context.OrderDetails.CountAsync();


            Assert.AreEqual(expectedOrderDetailCount, actualOrderDetailCount);
        }

        [Test]
        public async Task GetAllAsync_ReturnAllNotDeletedOrderDetails()
        {

            List<OrderDetail> expected = _contextData.OrderDetails;


            IEnumerable<OrderDetail> actual = await _orderDetailRepository.GetAllAsync();


            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}
