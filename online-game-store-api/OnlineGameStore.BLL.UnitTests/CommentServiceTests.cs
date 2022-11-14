using AutoMapper;
using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.Configurations.MapperConfigurations;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.UnitTests
{
    public class CommentServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<Comment>> _mockCommentRepository;
        private readonly IMapper _mapper;
        public CommentServiceTests()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mc => new MappingProfile(mc));
            _mapper = mapperConfiguration.CreateMapper();
        }

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCommentRepository = new Mock<IRepository<Comment>>();

            _mockUnitOfWork.Setup(x => x.CommentRepository).Returns(_mockCommentRepository.Object);
        }

        [Test]
        public async Task GetByGameKeyAsync_ValidGameKey_ReturnParentCommentList()
        {

            Mock<IRepository<Game>> gameRepoMock = new Mock<IRepository<Game>>();

            gameRepoMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Game, bool>>>()))
            .ReturnsAsync(() => new Game() { Comments = new List<Comment>(), DownloadPath = "example" });

            _mockUnitOfWork.Setup(x => x.GameRepository).Returns(gameRepoMock.Object);

            _mockCommentRepository
                .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
                .ReturnsAsync(() => new List<Comment>());

            CommentService commentService = new CommentService(_mockUnitOfWork.Object, _mapper);


            IEnumerable<CommentModel> comments = await commentService.GetByGameKeyAsync("game key");


            Assert.IsNotNull(comments);
            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
        }

        [Test]
        public async Task CreateAsync_ValidComment_ReturnNewComment()
        {

            _mockCommentRepository
                .Setup(x => x.CreateAsync(It.IsAny<Comment>()))
                .ReturnsAsync(() => new Comment());

            CommentService commentService = new CommentService(_mockUnitOfWork.Object, _mapper);
            CommentModel newCommentModel = new CommentModel();


            CommentModel actual = await commentService.CreateAsync(newCommentModel);


            Assert.IsNotNull(actual);

            _mockUnitOfWork.Verify(x => x.CommentRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.CommentRepository.CreateAsync(It.IsAny<Comment>()), Times.Once);
        }

        [Test]
        public void CreateAsync_InvalidComment_ThrowModelException()
        {

            _mockCommentRepository
                .Setup(x => x.CreateAsync(It.IsAny<Comment>()))
                .Throws(new Exception());

            CommentService commentService = new CommentService(_mockUnitOfWork.Object, _mapper);
            CommentModel newCommentModel = new CommentModel();


            Assert.ThrowsAsync<Exception>(() => commentService.CreateAsync(newCommentModel));

            _mockUnitOfWork.Verify(x => x.CommentRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.CommentRepository.CreateAsync(It.IsAny<Comment>()), Times.Once);
        }
    }
}
