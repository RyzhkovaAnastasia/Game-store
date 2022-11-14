using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OnlineGameStore.DAL.Context;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.UnitTests.SqlRepositoriesTests;
using System.Linq;

namespace OnlineGameStore.DAL.UnitTests.PipelineTests
{
    internal class PipelineTests
    {
        private readonly IGameStoreDBContext _context;
        private TestData _contextData;

        public PipelineTests()
        {
            DbContextOptionsBuilder<GameStoreDBContext> options = new DbContextOptionsBuilder<GameStoreDBContext>();
            options.UseInMemoryDatabase("OnlineGameStore");
            _context = new GameStoreDBContext(options.Options);
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
        public void Process_EmptyFilter_ReturnGamesInInput()
        {
            var pipe = new Pipelines.Pipelines.GamePipeline(new DTOs.GamesFilterComponents());
            var expected = _contextData.Games;

            var actual = pipe.Process(expected);

            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [Test]
        public void Process_NullFilter_ReturnGamesInInput()
        {
            var pipe = new Pipelines.Pipelines.GamePipeline(null);
            var expected = _contextData.Games;

            var actual = pipe.Process(expected);

            Assert.AreEqual(expected.Count(), actual.Count());
        }
    }
}
