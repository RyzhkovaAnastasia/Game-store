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
    public class GameRepositoryTests
    {
        protected IGameStoreDBContext _context;
        private readonly ISQLRepository<Game> _gameRepository;
        internal TestData _contextData;
        public GameRepositoryTests()
        {
            DbContextOptionsBuilder<GameStoreDBContext> options = new DbContextOptionsBuilder<GameStoreDBContext>();
            options.UseInMemoryDatabase("OnlineGameStore");
            _context = new GameStoreDBContext(options.Options);

            _gameRepository = new GameRepository(_context);
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
        public async Task CreateAsync_ValidGameEntity_ReturnGameEntity()
        {

            Game expected = new Game { Id = Guid.NewGuid(), Name = "Mario", Key = "mario" };


            Game actual = await _gameRepository.CreateAsync(expected);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateAsync_InsertExistingGameEntity_ThrowException()
        {
            Game existingEntity = _contextData.Games[0];

            Assert.CatchAsync<ArgumentException>(() => _gameRepository.CreateAsync(existingEntity));
        }

        [Test]
        public async Task CreateAsync_NullGameEntity_ReturnNull()
        {

            Game expected = null;


            Game actual = await _gameRepository.CreateAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_ValidGameEntity_ReturnGameEntity()
        {

            Game expected = _contextData.Games[0];
            expected.Name = "Another game";


            Game actual = await _gameRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_NullGameEntity_ReturnNull()
        {
            Game expected = null;

            Game actual = await _gameRepository.EditAsync(expected);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_NoOldEntity_ReturnNull()
        {
            Game expected = new Game() { Id = Guid.NewGuid() };

            Game actual = await _gameRepository.EditAsync(expected);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task EditFieldAsync_ChangeValidField_ReturnChangedEntity()
        {
            var expected = _contextData.Games[0];
            expected.IsDeleted = true;
            var actual = await _gameRepository.EditFieldAsync(expected, nameof(expected.IsDeleted));

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public async Task EditFieldAsync_NotExistingField_ReturnOldEntity()
        {
            var expected = _contextData.Games[0];
            expected.IsDeleted = true;

            var actual = await _gameRepository.EditFieldAsync(expected, "notExistingProp");

            expected.IsDeleted = false;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_GameEntityId_ReturnGameEntity()
        {
            Game expected = _contextData.Games[0];

            Game actual = await _gameRepository.GetByIdAsync(expected.Id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_NotExistingGameId_ReturnNull()
        {
            Game expected = null;
            Guid notExistingId = Guid.NewGuid();

            Game actual = await _gameRepository.GetByIdAsync(notExistingId);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_GameEntityId_ReturnGameEntity()
        {
            Game expected = _contextData.Games[0];

            Game actual = await _gameRepository.FindAsync(game => game.Id == expected.Id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NotExistingGameId_ReturnNull()
        {
            Game expected = null;
            Guid notExistingId = Guid.NewGuid();

            Game actual = await _gameRepository.FindAsync(game => game.Id == notExistingId);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NullPredicate_ReturnNull()
        {
            Game expected = null;

            Game actual = await _gameRepository.FindAsync(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAllAsync_ValidPredicate_ReturnGameList()
        {
            IEnumerable<Game> expected = _contextData.Games;

            var actual = await _gameRepository.GetAllAsync(game => true);

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetAllAsync_NullPredicate_ReturnAllGames()
        {
            IEnumerable<Game> expected = _contextData.Games;

            var actual = await _gameRepository.GetAllAsync(null);

            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [Test]
        public async Task DeleteAsync_ValidId_UpdateIsDeleted()
        {
            var expectedGamesCount = _contextData.Games.Count;
            await _gameRepository.DeleteAsync(_contextData.Games[0].Id);

            var actualGamesCount = _contextData.Games.Count;

            Assert.AreEqual(expectedGamesCount, actualGamesCount);
        }

        [Test]
        public async Task GetAllAsync_ReturnAllNotDeletedGames()
        {
            List<Game> expected = _contextData.Games;

            var actual = await _gameRepository.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetWhereAsNoTrackingAsync_ValidPredicate_ReturnCommentList()
        {
            var expected = _contextData.Games.Count;

            var actual = await _gameRepository.GetWhereAsNoTrackingAsync(game => game.Id != Guid.NewGuid());

            Assert.AreEqual(actual.Count(), expected);
        }
    }
}