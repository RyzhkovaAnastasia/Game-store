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
    internal class CommentRepositoryTests
    {
        private readonly IGameStoreDBContext _context;
        private readonly ISQLRepository<Comment> _commentRepository;
        private TestData _contextData;

        public CommentRepositoryTests()
        {
            DbContextOptionsBuilder<GameStoreDBContext> options = new DbContextOptionsBuilder<GameStoreDBContext>();
            options.UseInMemoryDatabase("OnlineGameStore");
            _context = new GameStoreDBContext(options.Options);
            _commentRepository = new SQLRepository<Comment>(_context);
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
        public async Task CreateAsync_ValidCommentEntity_ReturnCommentEntity()
        {

            Comment expected = new Comment { Id = Guid.NewGuid(), Name = "Mario", Body = "Wow!" };

            Comment actual = await _commentRepository.CreateAsync(expected);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateAsync_InsertExistingCommentEntity_ThrowException()
        {

            Comment existingEntity = _contextData.Comments[0];

            Assert.CatchAsync<ArgumentException>(() => _commentRepository.CreateAsync(existingEntity));
        }

        [Test]
        public async Task CreateAsync_NullCommentEntity_ReturnNull()
        {

            Comment expected = null;

            Comment actual = await _commentRepository.CreateAsync(expected);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_ValidCommentEntity_ReturnCommentEntity()
        {

            Comment expected = _contextData.Comments[0];
            expected.Name = "Another comment";

            Comment actual = await _commentRepository.EditAsync(expected);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditAsync_NullCommentEntity_ReturnNull()
        {
            Comment expected = null;
            Comment actual = await _commentRepository.EditAsync(expected);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditFieldAsync_ChangeValidField_ReturnChangedEntity()
        {
            Comment expected = _contextData.Comments[0];
            expected.IsDeleted = true;
            Comment actual = await _commentRepository.EditFieldAsync(expected, nameof(expected.IsDeleted));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task EditFieldAsync_NotExistingField_ReturnOldEntity()
        {
            Comment expected = _contextData.Comments[0];
            expected.IsDeleted = true;

            Comment actual = await _commentRepository.EditFieldAsync(expected, "notExistingProp");

            expected.IsDeleted = false;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_CommentEntityId_ReturnCommentEntity()
        {
            Comment expected = _contextData.Comments[0];

            Comment actual = await _commentRepository.GetByIdAsync(expected.Id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetByIsAsync_NotExistingCommentId_ReturnNull()
        {
            Comment expected = null;
            Guid notExistingId = Guid.NewGuid();

            Comment actual = await _commentRepository.GetByIdAsync(notExistingId);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_CommentEntityId_ReturnCommentEntity()
        {
            Comment expected = _contextData.Comments[0];

            Comment actual = await _commentRepository.FindAsync(comment => comment.Id == expected.Id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NotExistingCommentId_ReturnNull()
        {
            Comment expected = null;
            Guid notExistingId = Guid.NewGuid();

            Comment actual = await _commentRepository.FindAsync(comment => comment.Id == notExistingId);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task FindAsync_NullPredicate_ReturnNull()
        {
            Comment expected = null;

            Comment actual = await _commentRepository.FindAsync(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAllAsync_ValidPredicate_ReturnCommentList()
        {

            IEnumerable<Comment> expected = _contextData.Comments;

            IEnumerable<Comment> actual = await _commentRepository.GetAllAsync(comment => comment.Id != null);

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetWhereAsNoTrackingAsync_ValidPredicate_ReturnCommentList()
        {
            var expected = _contextData.Comments.Count;

            var actual = await _commentRepository.GetWhereAsNoTrackingAsync(comment => comment.Id != Guid.NewGuid());

            Assert.AreEqual(actual.Count(), expected);
        }

        [Test]
        public async Task GetWhere_NullPredicate_ReturnAll()
        {
            IEnumerable<Comment> expected = _contextData.Comments;

            IEnumerable<Comment> actual = await _commentRepository.GetAllAsync(null);

            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [Test]
        public async Task DeleteAsync_ValidId_DeleteEntity()
        {
            Comment commentToDelete = _contextData.Comments[^1];
            int expectedCommentCount = _contextData.Comments.Count - 1;

            await _commentRepository.DeleteAsync(commentToDelete.Id);

            int actualCommentCount = await _context.Comments.CountAsync();

            Assert.AreEqual(expectedCommentCount, actualCommentCount);
        }

        [Test]
        public async Task GetAllAsync_ReturnAllNotDeletedComments()
        {
            List<Comment> expected = _contextData.Comments;

            IEnumerable<Comment> actual = await _commentRepository.GetAllAsync();

            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}
