using AutoMapper;
using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.Configurations.MapperConfigurations;
using OnlineGameStore.BLL.CustomExceptions;
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
    public class GameServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<Game>> _mockGameRepository;
        private readonly IMapper _mapper;
        public GameServiceTests()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mc => new MappingProfile(mc));
            _mapper = mapperConfiguration.CreateMapper();
        }

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockGameRepository = new Mock<IRepository<Game>>();
            _mockUnitOfWork.Setup(x => x.GameRepository).Returns(_mockGameRepository.Object);
        }

        [Test]
        public async Task GetAll_ReturnGameList()
        {
            _mockGameRepository
                .Setup(x => x.GetAllAsync(null))
                .ReturnsAsync(() => new List<Game>());

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);

            var games = await gameService.GetAllAsync();

            Assert.IsNotNull(games);
            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.GetAllAsync(null), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnGame()
        {

            _mockGameRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(() => new Game());

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);


            GameModel game = await gameService.GetByIdAsync(Guid.NewGuid());


            Assert.IsNotNull(game);
            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.FindAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }

        [Test]
        public void GetByIdAsync_NotExistingId_ThrowNotFoundException()
        {

            _mockGameRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(() => null);

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);

            Assert.ThrowsAsync<NotFoundException>(() => gameService.GetByIdAsync(Guid.NewGuid()));
            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.FindAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }

        [Test]
        public async Task EditAsync_ValidGame_ReturnNewGameModel()
        {

            _mockGameRepository
                .Setup(x => x.EditAsync(It.IsAny<Game>()))
                .ReturnsAsync(() => new Game());

            _mockGameRepository
               .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Game, bool>>>()))
               .ReturnsAsync(() => new List<Game>());

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);
            GameModel editedGameModel = new GameModel() { Name = "test", Key = "test" };


            GameModel games = await gameService.EditAsync(editedGameModel);


            Assert.IsNotNull(games);
            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.EditAsync(It.IsAny<Game>()), Times.Once);
        }

        [Test]
        public void EditAsync_InvalidGameId_ThrowException()
        {

            _mockGameRepository
                .Setup(x => x.EditAsync(It.IsAny<Game>()))
                .ReturnsAsync(() => null);

            _mockGameRepository
              .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Game, bool>>>()))
              .ReturnsAsync(() => new List<Game>());

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);
            GameModel editedGameModel = new GameModel() { Id = Guid.NewGuid(), Key = "key", Genres = new List<GenreModel>() };



            Assert.ThrowsAsync<NotFoundException>(() => gameService.EditAsync(editedGameModel));
            _mockUnitOfWork.Verify(x => x.GameRepository.EditAsync(It.IsAny<Game>()), Times.Once);
        }

        [Test]
        public void EditAsync_InvalidGame_ThrowException()
        {

            _mockGameRepository
                .Setup(x => x.EditAsync(It.IsAny<Game>()))
                .Throws(new Exception());

            _mockGameRepository
               .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Game, bool>>>()))
               .ReturnsAsync(() => new List<Game>());


            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);
            GameModel updatedGameModel = new GameModel() { Name = "Test", Key = "test" };


            Assert.ThrowsAsync<Exception>(() => gameService.EditAsync(updatedGameModel));

            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.EditAsync(It.IsAny<Game>()), Times.Once);
        }

        [Test]
        public void EditAsync_ExistingKeyGame_ThrowException()
        {

            GameModel updatedGameModel = new GameModel() { Key = "Key", Id = Guid.NewGuid() };

            _mockGameRepository
                 .Setup(x => x.GetWhereAsNoTrackingAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                 .ReturnsAsync(() => new List<Game>() { new Game() { Key = "Key", Id = Guid.NewGuid() } });

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);

            Assert.ThrowsAsync<ModelException>(() => gameService.EditAsync(updatedGameModel));

            _mockUnitOfWork.Verify(x => x.GameRepository.GetWhereAsNoTrackingAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidGame_ReturnNewGameModel()
        {

            _mockGameRepository
                .Setup(x => x.CreateAsync(It.IsAny<Game>()))
                .ReturnsAsync(() => new Game());

            _mockGameRepository
                .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(() => new List<Game>());

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);
            GameModel newGameModel = new GameModel() { Name = "test", Key = "test", Genres = new List<GenreModel>() };


            GameModel games = await gameService.CreateAsync(newGameModel);


            Assert.IsNotNull(games);
            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.CreateAsync(It.IsAny<Game>()), Times.Once);
        }

        [Test]
        public void CreateAsync_InvalidGame_ThrowException()
        {

            _mockGameRepository
                .Setup(x => x.CreateAsync(It.IsAny<Game>()))
                .Throws(new Exception());

            _mockGameRepository
               .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Game, bool>>>()))
               .ReturnsAsync(() => new List<Game>());

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);
            GameModel newGameModel = new GameModel() { Name = "test", Key = "test" };


            Assert.ThrowsAsync<Exception>(() => gameService.CreateAsync(newGameModel));

            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.CreateAsync(It.IsAny<Game>()), Times.Once);
        }


        [Test]
        public void CreateAsync_ExistingKeyGame_ThrowModelException()
        {

            GameModel updatedGameModel = new GameModel() { Key = "Key", Id = Guid.NewGuid() };

            _mockGameRepository
                .Setup(x => x.GetWhereAsNoTrackingAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(() => new List<Game>() { new Game() { Key = "Key", Id = Guid.NewGuid() } });

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);

            Assert.ThrowsAsync<ModelException>(() => gameService.CreateAsync(updatedGameModel));

            _mockUnitOfWork.Verify(x => x.GameRepository.GetWhereAsNoTrackingAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }

        [Test]
        public async Task GetByKeyAsync_ValidGameKey_ReturnGameModel()
        {

            _mockGameRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(() => new Game() { Key = "key" });

            var mockGenreRepo = new Mock<IRepository<Genre>>();
            mockGenreRepo
                .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .ReturnsAsync(() => null);

            _mockUnitOfWork.Setup(x => x.GenreRepository).Returns(mockGenreRepo.Object);

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);
            string key = "key";

            GameModel games = await gameService.GetByKeyAsync(key);


            Assert.IsNotNull(games);
            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.FindAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ValidGame_DeleteGame()
        {

            _mockGameRepository
                .Setup(x => x.SoftDeleteAsync(It.IsAny<Guid>()));

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);


            await gameService.DeleteAsync(Guid.NewGuid());


            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.SoftDeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void DeleteAsync_InvalidGame_ThrowException()
        {

            _mockGameRepository
                .Setup(x => x.SoftDeleteAsync(It.IsAny<Guid>()))
                .Throws(new Exception());

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);


            Assert.ThrowsAsync<Exception>(() => gameService.DeleteAsync(Guid.NewGuid()));

            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.SoftDeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void GetByKeyAsync_InvalidGameKey_ThrowException()
        {

            _mockGameRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(() => null);

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);
            string key = "game key";



            Assert.ThrowsAsync<NotFoundException>(() => gameService.GetByKeyAsync(key));
            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.FindAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }

        [Test]
        public async Task GetGameNumberAsync_ReturnNumberOfGames()
        {

            _mockGameRepository
                .Setup(x => x.GetAllAsync(null))
                .ReturnsAsync(() => new List<Game>());

            int excpected = 0;

            GameService gameService = new GameService(_mockUnitOfWork.Object, _mapper);


            int actual = await gameService.GetGameNumberAsync();


            Assert.AreEqual(excpected, actual);
            _mockUnitOfWork.Verify(x => x.GameRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GameRepository.GetAllAsync(null), Times.Once);
        }
    }
}