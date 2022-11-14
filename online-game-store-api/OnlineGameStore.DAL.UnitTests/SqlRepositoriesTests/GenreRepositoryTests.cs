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
    internal class GenreRepositoryTests
    {
        private readonly IGameStoreDBContext _context;
        private readonly ISQLRepository<Genre> _genreRepository;
        private TestData _contextData;

        public GenreRepositoryTests()
        {
            DbContextOptionsBuilder<GameStoreDBContext> options = new DbContextOptionsBuilder<GameStoreDBContext>();
            options.UseInMemoryDatabase("OnlineGameStore");
            _context = new GameStoreDBContext(options.Options);
            _genreRepository = new SQLRepository<Genre>(_context);
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
        public async Task CreateAsync_ValidGenreEntity_ReturnGenreEntity()
        {

            Genre expected = new Genre { Id = Guid.NewGuid(), Name = "Some genre" };


            Genre actual = await _genreRepository.CreateAsync(expected);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task CreateAsync_InsertExistingGenreEntity_ThrowException()
        {

            Genre existingEntity = _contextData.Genres[0];

            Assert.CatchAsync<ArgumentException>(() => _genreRepository.CreateAsync(existingEntity));
        }

        [Test]
        public async Task CreateAsync_NullGenreEntity_ReturnNull()
        {

            Genre expected = null;


            Genre actual = await _genreRepository.CreateAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_ValidGenreEntity_ReturnGenreEntity()
        {

            Genre expected = _contextData.Genres[0];
            expected.Name = "Another genre";


            Genre actual = await _genreRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_NullGenreEntity_ReturnNull()
        {

            Genre expected = null;


            Genre actual = await _genreRepository.EditAsync(expected);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_GenreEntityId_ReturnGenreEntity()
        {

            Genre expected = _contextData.Genres[0];


            Genre actual = await _genreRepository.GetByIdAsync(expected.Id);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_NotExistingGenreId_ReturnNull()
        {

            Genre expected = null;
            Guid notExistingId = Guid.NewGuid();

            Genre actual = await _genreRepository.GetByIdAsync(notExistingId);



            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_GenreEntityId_ReturnGenreEntity()
        {

            Genre expected = _contextData.Genres[0];


            Genre actual = await _genreRepository.FindAsync(genre => genre.Id == expected.Id);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NotExistingGenreId_ReturnNull()
        {

            Genre expected = null;
            Guid notExistingId = Guid.NewGuid();


            Genre actual = await _genreRepository.FindAsync(genre => genre.Id == notExistingId);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NullPredicate_ReturnNull()
        {

            Genre expected = null;


            Genre actual = await _genreRepository.FindAsync(null);


            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetWhere_ValidPredicate_ReturnGenreList()
        {

            IEnumerable<Genre> expected = _contextData.Genres;


            IEnumerable<Genre> actual = await _genreRepository.GetAllAsync(genre => genre.Id != null);


            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetWhere_NullPredicate_ReturnAll()
        {

            IEnumerable<Genre> expected = _contextData.Genres;


            IEnumerable<Genre> actual = await _genreRepository.GetAllAsync(null);


            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [Test]
        public async Task DeleteAsync_ValidId_UpdateIsDeleted()
        {

            Genre genreToDelete = _contextData.Genres[^1];
            int expectedGenreCount = _contextData.Genres.Count - 1;


            await _genreRepository.DeleteAsync(genreToDelete.Id);


            int actualGenreCount = await _context.Genres.CountAsync();


            Assert.AreEqual(expectedGenreCount, actualGenreCount);
        }

        [Test]
        public async Task GetAllAsync_ReturnAllNotDeletedGenres()
        {

            List<Genre> expected = _contextData.Genres;


            IEnumerable<Genre> actual = await _genreRepository.GetAllAsync();


            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}
