using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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
    internal class PublisherRepositoryTests
    {
        private readonly IGameStoreDBContext _context;
        private readonly ISQLRepository<Publisher> _publisherRepository;
        private TestData _contextData;

        public PublisherRepositoryTests()
        {
            DbContextOptionsBuilder<GameStoreDBContext> options = new DbContextOptionsBuilder<GameStoreDBContext>();
            options.UseInMemoryDatabase("OnlineGameStore");
            _context = new GameStoreDBContext(options.Options);
            _publisherRepository = new SQLRepository<Publisher>(_context);
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
        public async Task CreateAsync_ValidPublisherEntity_ReturnPublisherEntity()
        {

            Publisher expected = new Publisher { Id = Guid.NewGuid(), CompanyName = "Some publisher" };


            Publisher actual = await _publisherRepository.CreateAsync(expected);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateAsync_InsertExistingPublisherEntity_ThrowException()
        {

            Publisher existingEntity = _contextData.Publishers[0];

            Assert.CatchAsync<ArgumentException>(() => _publisherRepository.CreateAsync(existingEntity));
        }

        [Test]
        public async Task CreateAsync_NullPublisherEntity_ReturnNull()
        {

            Publisher expected = null;


            Publisher actual = await _publisherRepository.CreateAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_ValidPublisherEntity_ReturnPublisherEntity()
        {

            Publisher expected = _contextData.Publishers[0];
            expected.CompanyName = "Another publisher";


            Publisher actual = await _publisherRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_NullPublisherEntity_ReturnNull()
        {

            Publisher expected = null;


            Publisher actual = await _publisherRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_PublisherEntityId_ReturnPublisherEntity()
        {

            Publisher expected = _contextData.Publishers[0];


            Publisher actual = await _publisherRepository.GetByIdAsync(expected.Id);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_NotExistingPublisherId_ReturnNull()
        {

            Publisher expected = null;
            Guid notExistingId = Guid.NewGuid();

            Publisher actual = await _publisherRepository.GetByIdAsync(notExistingId);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_PublisherEntityId_ReturnPublisherEntity()
        {

            Publisher expected = _contextData.Publishers[0];


            Publisher actual = await _publisherRepository.FindAsync(publisher => publisher.Id == expected.Id);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NotExistingPublisherId_ReturnNull()
        {

            Publisher expected = null;
            Guid notExistingId = Guid.NewGuid();

            Publisher actual = await _publisherRepository.FindAsync(publisher => publisher.Id == notExistingId);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NullPredicate_ReturnNull()
        {

            Publisher expected = null;


            Publisher actual = await _publisherRepository.FindAsync(null);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetWhere_ValidPredicate_ReturnPublisherList()
        {

            IEnumerable<Publisher> expected = _contextData.Publishers;


            IEnumerable<Publisher> actual = await _publisherRepository.GetAllAsync(publisher => publisher.Id != null);


            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetWhere_NullPredicate_ReturnAll()
        {

            IEnumerable<Publisher> expected = _contextData.Publishers;


            IEnumerable<Publisher> actual = await _publisherRepository.GetAllAsync(null);


            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [Test]
        public async Task DeleteAsync_ValidId_UpdateIsDeleted()
        {

            Publisher publisherToDelete = _contextData.Publishers[^1];
            int expectedPublisherCount = _contextData.Publishers.Count - 1;


            await _publisherRepository.DeleteAsync(publisherToDelete.Id);


            int actualPublisherCount = await _context.Publishers.CountAsync();


            Assert.AreEqual(expectedPublisherCount, actualPublisherCount);
        }

        [Test]
        public async Task GetAllAsync_ReturnAllNotDeletedPublishers()
        {
            List<Publisher> expected = _contextData.Publishers;

            IEnumerable<Publisher> actual = await _publisherRepository.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}