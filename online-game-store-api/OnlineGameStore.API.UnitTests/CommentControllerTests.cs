using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.CustomExceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.API.UnitTests
{
    public class CommentControllerTests
    {
        private Mock<ICommentService> _mockCommentService;

        [SetUp]
        public void Setup()
        {
            _mockCommentService = new Mock<ICommentService>();
        }

        [Test]
        public async Task GetAllAsyncCommentsAsync_ExecOnce()
        {
            _mockCommentService.Setup(x => x.GetByGameKeyAsync(It.IsAny<string>())).ReturnsAsync(() => new List<CommentModel>());
            CommentController commentController = new CommentController(_mockCommentService.Object);

            var actual = await commentController.GetCommentsByGameKeyAsync("");

            _mockCommentService.Verify(x => x.GetByGameKeyAsync(""), Times.Once);
        }

        [Test]
        public async Task GetByKeyAsync_ValidKey_ExecOnce()
        {
            _mockCommentService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new CommentModel());
            CommentController commentController = new CommentController(_mockCommentService.Object);
            Guid commentId = Guid.NewGuid();

            await commentController.GetCommentByIdAsync("", commentId);

            _mockCommentService.Verify(x => x.GetByIdAsync(commentId), Times.Once);
        }

        [Test]
        public void GetByKeyAsync_NotExistingKey_ThrowNotFoundException()
        {
            _mockCommentService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Throws(() => new NotFoundException());

            CommentController commentController = new CommentController(_mockCommentService.Object);
            Guid commentId = Guid.NewGuid();

            Assert.ThrowsAsync<NotFoundException>(() => commentController.GetCommentByIdAsync("", commentId));

            _mockCommentService.Verify(x => x.GetByIdAsync(commentId), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidCommentModel_ExecOnce()
        {
            _mockCommentService.Setup(x => x.CreateAsync(It.IsAny<CommentModel>())).ReturnsAsync(() => new CommentModel());
            CommentController commentController = new CommentController(_mockCommentService.Object);
            CommentModel comment = new CommentModel();

            await commentController.CreateAsync(comment);

            _mockCommentService.Verify(x => x.CreateAsync(comment), Times.Once);
        }

        [Test]
        public void CreateAsync_NotUniqueKey_ThrowModelException()
        {
            _mockCommentService.Setup(x => x.CreateAsync(It.IsAny<CommentModel>()))
                .Throws(() => new ModelException());

            CommentController commentController = new CommentController(_mockCommentService.Object);
            CommentModel comment = new CommentModel();

            Assert.ThrowsAsync<ModelException>(() => commentController.CreateAsync(comment));

            _mockCommentService.Verify(x => x.CreateAsync(comment), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ValidCommentModel_ExecOnce()
        {
            _mockCommentService.Setup(x => x.DeleteAsync(It.IsAny<Guid>()));
            CommentController commentController = new CommentController(_mockCommentService.Object);
            Guid commentId = Guid.NewGuid();

            await commentController.DeleteAsync(commentId);

            _mockCommentService.Verify(x => x.DeleteAsync(commentId), Times.Once);
        }

        [Test]
        public void DeleteAsync_NotUniqueKey_ThrowModelException()
        {
            _mockCommentService.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .Throws(() => new ModelException());

            CommentController commentController = new CommentController(_mockCommentService.Object);
            Guid commentId = Guid.NewGuid();

            Assert.ThrowsAsync<ModelException>(() => commentController.DeleteAsync(commentId));

            _mockCommentService.Verify(x => x.DeleteAsync(commentId), Times.Once);
        }
    }
}

