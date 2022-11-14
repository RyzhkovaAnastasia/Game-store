using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OnlineGameStore.DAL.Context;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Pipelines.SortPipelineSteps;
using OnlineGameStore.DAL.UnitTests.SqlRepositoriesTests;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.UnitTests.PipelineTests
{
    internal class SortPipelineTests
    {
        private readonly IGameStoreDBContext _context;
        private TestData _contextData;

        public SortPipelineTests()
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
        public void SortByCommentNumberProcess_Ascending_ReturnGamesInOrder()
        {
            var pipe = new GameSortByCommentNumberPipeline();
            var expected = new List<Game>() { _contextData.Games[1] };

            var actual = pipe.Process(_contextData.Games);

            Assert.AreEqual(expected[0].Key, actual[0].Key);
        }

        [Test]
        public void SortByCommentNumberProcess_Descending_ReturnGamesInOrder()
        {
            var pipe = new GameSortByCommentNumberPipeline(false);
            var expected = new List<Game>() { _contextData.Games[0] };

            var actual = pipe.Process(_contextData.Games);

            Assert.AreEqual(expected[0].Key, actual[0].Key);
        }

        [Test]
        public void SortByDateAddToStoreProcess_Ascending_ReturnGamesInOrder()
        {
            var pipe = new GameSortByDateAddToStorePipeline();
            var expected = new List<Game>() { _contextData.Games[0] };

            var actual = pipe.Process(_contextData.Games);

            Assert.AreEqual(expected[0].Key, actual[0].Key);
        }

        [Test]
        public void SortByDateAddToStoreProcess_Descending_ReturnGamesInOrder()
        {
            var pipe = new GameSortByDateAddToStorePipeline(false);
            var expected = new List<Game>() { _contextData.Games[1] };

            var actual = pipe.Process(_contextData.Games);

            Assert.AreEqual(expected[0].Key, actual[0].Key);
        }

        [Test]
        public void SortByMostPopularProcess_Ascending_ReturnGamesInOrder()
        {
            var pipe = new GameSortByMostPopularPipeline();
            var expected = new List<Game>() { _contextData.Games[0] };

            var actual = pipe.Process(_contextData.Games);

            Assert.AreEqual(expected[0].Key, actual[0].Key);
        }

        [Test]
        public void SortByMostPopularProcess_Descending_ReturnGamesInOrder()
        {
            var pipe = new GameSortByMostPopularPipeline(false);
            var expected = new List<Game>() { _contextData.Games[0] };

            var actual = pipe.Process(_contextData.Games);

            Assert.AreEqual(expected[0].Key, actual[0].Key);
        }

        [Test]
        public void SortByPriceProcess_Ascending_ReturnGamesInOrder()
        {
            var pipe = new GameSortByPricePipeline();
            var expected = new List<Game>() { _contextData.Games[0] };

            var actual = pipe.Process(_contextData.Games);

            Assert.AreEqual(expected[0].Key, actual[0].Key);
        }

        [Test]
        public void SortByPriceProcess_Descending_ReturnGamesInOrder()
        {
            var pipe = new GameSortByPricePipeline(false);
            var expected = new List<Game>() { _contextData.Games[1] };

            var actual = pipe.Process(_contextData.Games);

            Assert.AreEqual(expected[0].Key, actual[0].Key);
        }

        [Test]
        public void SortSkipProcess_ReturnGamesInOrder()
        {
            var pipe = new GameSortByPricePipeline(false);
            var expected = _contextData.Games;

            var actual = pipe.Process(_contextData.Games);

            Assert.AreEqual(expected.Count(), actual.Count());
        }
    }
}
