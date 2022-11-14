using MongoDB.Driver;
using NUnit.Framework;
using OnlineGameStore.DAL.Context;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Repositories;
using OnlineGameStore.DAL.UnitTests.SqlRepositoriesTests;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.UnitTests.MongoDBCollectionsTests
{
    internal class ProductCollectionTests
    {
        private readonly INorthwindDBContext _context;
        private readonly IMongoRepository<Game> _gameRepository;
        private TestData _contextData;

        public ProductCollectionTests()
        {
            string TestDataConnectionString = "mongodb://localhost:27017/Northwind-Test";
            var client = new MongoClient(TestDataConnectionString);
            _context = new NorthwindDBContext(client, "Northwind-Test");
            _gameRepository = new MongoRepository<Game>(_context, "products");
        }

        [SetUp]
        public void Setup()
        {
            _contextData = new TestData();
            _context.GetCollection<Game>("products").InsertMany(_contextData.Games);
        }

        [TearDown]
        public void Down()
        {
            _context.GetCollection<Game>("products").DeleteMany(_ => true);
        }


        [Test]
        public async Task GetAllAsync_ReturnGame()
        {
            var expected = _contextData.Games;

            var actual = await _gameRepository.GetAllAsync();

            Assert.AreEqual(actual.Count(), expected.Count());
        }

        [Test]
        public async Task GetAllAsync_ExistingKey_ReturnGames()
        {
            var expected = _contextData.Games[0].Key;

            var actual = await _gameRepository.GetAllAsync(x => x.Key == expected);

            Assert.AreEqual(actual.FirstOrDefault().Key, expected);
        }

        [Test]
        public async Task GetAllAsync_NotExistingKey_ReturnNull()
        {
            var expected = "notExistingKey";

            var actual = await _gameRepository.GetAllAsync(x => x.Key == expected);

            Assert.IsNull(actual.FirstOrDefault());
        }


        [Test]
        public async Task EditFieldAsync_UpdateKey_ReturnGame()
        {
            var entity = new Game() { Key = "sims2" };
            var expected = "sims2";

            var actual = await _gameRepository.EditFieldAsync(entity, nameof(entity.Key));

            Assert.AreEqual(actual.Key, expected);
        }

        [Test]
        public async Task EditFieldAsync_NotExistingField_ReturnNoChangedEntity()
        {
            var entity = new Game() { Key = "sims2" };
            var expected = "sims2";

            var actual = await _gameRepository.EditFieldAsync(entity, "notExistingField");

            Assert.AreEqual(actual.Key, expected);
        }

        [Test]
        public async Task FindAsync_ValidFilter_ReturnGame()
        {
            var entity = _contextData.Games[0];
            var expected = entity.Key;

            var actual = await _gameRepository.FindAsync(x => x.Key == entity.Key);

            Assert.AreEqual(actual.Key, expected);
        }

        [Test]
        public async Task FindAsync_NullFilter_ReturnNull()
        {

            var actual = await _gameRepository.FindAsync(null);

            Assert.IsNull(actual);
        }

    }
}
