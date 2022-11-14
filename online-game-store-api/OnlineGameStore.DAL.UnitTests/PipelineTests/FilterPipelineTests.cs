using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OnlineGameStore.DAL.Context;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Pipelines.PipelineSteps;
using OnlineGameStore.DAL.UnitTests.SqlRepositoriesTests;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.UnitTests.PipelineTests
{
    internal class FilterPipelineTests
    {
        private readonly IGameStoreDBContext _context;
        private TestData _contextData;

        public FilterPipelineTests()
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
        public void GameGenreProcess_ValidGenres_ReturnGamesContainGenres()
        {
            var pipe = new GameGenresPipeline(_contextData.Genres);
            var expected = _contextData.Games.ToList();

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GameGenreProcess_EmptyGenres_ReturnAllGames()
        {
            var pipe = new GameGenresPipeline(new List<Genre>());
            var expected = _contextData.Games.ToList();

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GameMaxPriceProcess_ValidPrice_ReturnFilteredGames()
        {
            var pipe = new GameMaxPricePipeline(1000);
            var expected = _contextData.Games.ToList();

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GameMaxPriceProcessProcess_NullPrice_ReturnAllGames()
        {
            var pipe = new GameMaxPricePipeline(null);
            var expected = _contextData.Games.ToList();

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GameMinPriceProcess_ValidPrice_ReturnFilteredGames()
        {
            var pipe = new GameMinPricePipeline(0);
            var expected = _contextData.Games.ToList();

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GameMinPriceProcessProcess_NullPrice_ReturnAllGames()
        {
            var pipe = new GameMinPricePipeline(null);
            var expected = _contextData.Games.ToList();

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GameMinPriceProcessProcess_LessZeroPrice_ReturnAllGames()
        {
            var pipe = new GameMinPricePipeline(-10);
            var expected = _contextData.Games.ToList();

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GameMaxPriceProcessProcess_LessZeroPrice_ReturnAllGames()
        {
            var pipe = new GameMaxPricePipeline(-10);
            var expected = 0;

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual.Count());
        }


        [Test]
        public void GameNameProcess_ValidName_ReturnFilteredGames()
        {
            var pipe = new GameNamePipeline("sims");
            var expected = new List<Game>() { _contextData.Games.First() };

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected[0].Name, actual[0].Name);
        }

        [Test]
        public void GameNameProcess_InvalidName_ReturnFilteredGames()
        {
            var pipe = new GameNamePipeline("a");
            var expected = _contextData.Games.ToList();

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GameNameProcess_NullName_ReturnFilteredGames()
        {
            var pipe = new GameNamePipeline(null);
            var expected = _contextData.Games.ToList();

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GamePlatformsProcess_NullPlatform_ReturnAllGames()
        {
            var pipe = new GamePlatformsPipeline(null);
            var expected = _contextData.Games.ToList();

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GamePlatformsProcess_ValidPlatforms_ReturnFilteredGames()
        {
            var pipe = new GamePlatformsPipeline(new List<PlatformType>() { _contextData.PlatformTypes[0] });
            var expected = _contextData.Games[0];

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual[0]);
        }

        [Test]
        public void GamePublishedProcess_LastWeek_ReturnFilteredGames()
        {
            var pipe = new GamePublishedPipeline(Common.Enums.GamePublishedDate.lastWeek);

            var expected = _contextData.Games[0];

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected, actual[0]);
        }

        [Test]
        public void GamePublishedProcess_AllTime_ReturnFilteredGames()
        {
            var pipe = new GamePublishedPipeline();
            var expected = _contextData.Games;

            var actual = pipe.Process(_contextData.Games.ToList());

            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [Test]
        public void GamePublishersProcess_ValidPublisher_ReturnFilteredGames()
        {
            var pipe = new GamePublishersPipeline(new List<Publisher>() { _contextData.Publishers[1] });
            var expected = 0;

            var actual = pipe.Process(_contextData.Games);

            Assert.AreEqual(expected, actual.Count());
        }

        [Test]
        public void GamePublishersProcess_NullPublisher_ReturnAllGames()
        {
            var pipe = new GamePublishersPipeline(null);
            var expected = _contextData.Games;

            var actual = pipe.Process(expected);

            Assert.AreEqual(expected.Count(), actual.Count());
        }
    }
}
