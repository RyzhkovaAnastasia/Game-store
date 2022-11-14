using Microsoft.EntityFrameworkCore;
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
    internal class PlatformTypeRepositoryTests
    {
        private readonly IGameStoreDBContext _context;
        private readonly ISQLRepository<PlatformType> _platformTypeRepository;
        private TestData _contextData;

        public PlatformTypeRepositoryTests()
        {
            DbContextOptionsBuilder<GameStoreDBContext> options = new DbContextOptionsBuilder<GameStoreDBContext>();
            options.UseInMemoryDatabase("OnlineGameStore");
            _context = new GameStoreDBContext(options.Options);
            _platformTypeRepository = new SQLRepository<PlatformType>(_context);
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
        public async Task CreateAsync_ValidPlatformTypeEntity_ReturnPlatformTypeEntity()
        {

            PlatformType expected = new PlatformType { Id = Guid.NewGuid(), Type = "Some platformType" };


            PlatformType actual = await _platformTypeRepository.CreateAsync(expected);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateAsync_InsertExistingPlatformTypeEntity_ThrowException()
        {

            PlatformType existingEntity = _contextData.PlatformTypes[0];

            Assert.CatchAsync<ArgumentException>(() => _platformTypeRepository.CreateAsync(existingEntity));
        }

        [Test]
        public async Task CreateAsync_NullPlatformTypeEntity_ReturnNull()
        {

            PlatformType expected = null;


            PlatformType actual = await _platformTypeRepository.CreateAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_ValidPlatformTypeEntity_ReturnPlatformTypeEntity()
        {

            PlatformType expected = _contextData.PlatformTypes[0];
            expected.Type = "Another platformType";


            PlatformType actual = await _platformTypeRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_NullPlatformTypeEntity_ReturnNull()
        {

            PlatformType expected = null;


            PlatformType actual = await _platformTypeRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_PlatformTypeEntityId_ReturnPlatformTypeEntity()
        {

            PlatformType expected = _contextData.PlatformTypes[0];


            PlatformType actual = await _platformTypeRepository.GetByIdAsync(expected.Id);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_NotExistingPlatformTypeId_ReturnNull()
        {

            PlatformType expected = null;
            Guid notExistingId = Guid.NewGuid();

            PlatformType actual = await _platformTypeRepository.GetByIdAsync(notExistingId);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_PlatformTypeEntityId_ReturnPlatformTypeEntity()
        {

            PlatformType expected = _contextData.PlatformTypes[0];


            PlatformType actual = await _platformTypeRepository.FindAsync(platformType => platformType.Id == expected.Id);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NotExistingPlatformTypeId_ReturnNull()
        {

            PlatformType expected = null;
            Guid notExistingId = Guid.NewGuid();


            PlatformType actual = await _platformTypeRepository.FindAsync(platformType => platformType.Id == notExistingId);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NullPredicate_ReturnNull()
        {

            PlatformType expected = null;


            PlatformType actual = await _platformTypeRepository.FindAsync(null);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetWhere_ValidPredicate_ReturnPlatformTypeList()
        {

            IEnumerable<PlatformType> expected = _contextData.PlatformTypes;


            IEnumerable<PlatformType> actual = await _platformTypeRepository.GetAllAsync(platformType => platformType.Id != null);


            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetWhere_NullPredicate_ReturnAllList()
        {

            IEnumerable<PlatformType> expected = _contextData.PlatformTypes;


            IEnumerable<PlatformType> actual = await _platformTypeRepository.GetAllAsync(null);


            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [Test]
        public async Task DeleteAsync_ValidId_UpdateIsDeleted()
        {

            PlatformType platformTypeToDelete = _contextData.PlatformTypes[^1];
            int expectedPlatformTypeCount = _contextData.PlatformTypes.Count - 1;


            await _platformTypeRepository.DeleteAsync(platformTypeToDelete.Id);


            int actualPlatformTypeCount = await _context.PlatformTypes.CountAsync();


            Assert.AreEqual(expectedPlatformTypeCount, actualPlatformTypeCount);
        }

        [Test]
        public async Task GetAllAsync_ReturnAllNotDeletedPlatformTypes()
        {

            List<PlatformType> expected = _contextData.PlatformTypes;


            IEnumerable<PlatformType> actual = await _platformTypeRepository.GetAllAsync();


            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}
