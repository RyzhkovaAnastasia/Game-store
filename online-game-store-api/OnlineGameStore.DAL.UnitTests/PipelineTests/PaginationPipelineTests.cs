using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OnlineGameStore.DAL.Context;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Pipelines.PaginationPipelineSteps;
using OnlineGameStore.DAL.UnitTests.SqlRepositoriesTests;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.UnitTests.PipelineTests
{
    internal class PaginationPipelineTests
    {
        private readonly IGameStoreDBContext _context;
        private TestData _contextData;

        public PaginationPipelineTests()
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
        public void PaginationProcess_ItemsOnPage1_Page2_ReturnGamesOnPage()
        {
            var pipe = new PaginationPipeline(new DTOs.GamesPagination() { CurrentPage = 2, ItemsPerPage = 1 });
            var expected = new List<Game>() { _contextData.Games[1] };

            var actual = pipe.Process(_contextData.Games.ToList()).ToList();

            Assert.AreEqual(expected[0].Key, actual[0].Key);
        }

        [Test]
        public void PaginationProcess_AfterLastPage_ReturnGamesOnLastPage()
        {
            var pipe = new PaginationPipeline(new DTOs.GamesPagination() { CurrentPage = 3, ItemsPerPage = 1 });
            var expected = 1;

            var actual = pipe.Process(_contextData.Games.ToList()).ToList();

            Assert.AreEqual(expected, actual.Count());
        }

        [Test]
        public void PaginationProcess_InvalidPage_ReturnGamesOnFirstPage()
        {
            var pipe = new PaginationPipeline(new DTOs.GamesPagination() { CurrentPage = -3, ItemsPerPage = 1 });
            var expected = new List<Game>() { _contextData.Games[0] };

            var actual = pipe.Process(_contextData.Games.ToList()).ToList();

            Assert.AreEqual(expected[0].Key, actual[0].Key);
        }

        [Test]
        public void PaginationProcess_NullPage_ReturnGames()
        {
            var pipe = new PaginationPipeline(null);
            var expected = _contextData.Games;

            var actual = pipe.Process(_contextData.Games.ToList()).ToList();

            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [Test]
        public void PaginationProcess_EmptyGameList_ReturnInput()
        {
            var pipe = new PaginationPipeline(new DTOs.GamesPagination());
            var expected = new List<Game>();

            var actual = pipe.Process(expected).ToList();

            Assert.AreEqual(expected.Count(), actual.Count());
        }
    }

}
