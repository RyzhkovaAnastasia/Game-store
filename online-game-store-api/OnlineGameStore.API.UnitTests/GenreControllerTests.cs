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
    public class GenreControllerTests
    {
        private Mock<IGenreService> _mockGenreService;

        [SetUp]
        public void Setup()
        {
            _mockGenreService = new Mock<IGenreService>();
        }

        [Test]
        public async Task GetAllAsyncGenresAsync_ExecOnce()
        {
            _mockGenreService.Setup(x => x.GetAllAsync()).ReturnsAsync(() => new List<GenreModel>());
            GenreController genreController = new GenreController(_mockGenreService.Object);

            var actual = await genreController.GetAllAsync();

            _mockGenreService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetByKeyAsync_ValidKey_ExecOnce()
        {
            _mockGenreService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new GenreModel());
            GenreController genreController = new GenreController(_mockGenreService.Object);
            Guid genreId = Guid.NewGuid();

            await genreController.GetByIdAsync(genreId);

            _mockGenreService.Verify(x => x.GetByIdAsync(genreId), Times.Once);
        }

        [Test]
        public void GetByKeyAsync_NotExistingKey_ThrowNotFoundException()
        {
            _mockGenreService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Throws(() => new NotFoundException());

            GenreController genreController = new GenreController(_mockGenreService.Object);
            Guid genreId = Guid.NewGuid();

            Assert.ThrowsAsync<NotFoundException>(() => genreController.GetByIdAsync(genreId));

            _mockGenreService.Verify(x => x.GetByIdAsync(genreId), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidGenreModel_ExecOnce()
        {
            _mockGenreService.Setup(x => x.CreateAsync(It.IsAny<GenreModel>())).ReturnsAsync(() => new GenreModel());
            GenreController genreController = new GenreController(_mockGenreService.Object);
            GenreModel genre = new GenreModel();

            await genreController.CreateAsync(genre);

            _mockGenreService.Verify(x => x.CreateAsync(genre), Times.Once);
        }

        [Test]
        public void CreateAsync_NotUniqueKey_ThrowModelException()
        {
            _mockGenreService.Setup(x => x.CreateAsync(It.IsAny<GenreModel>()))
                .Throws(() => new ModelException());

            GenreController genreController = new GenreController(_mockGenreService.Object);
            GenreModel genre = new GenreModel();

            Assert.ThrowsAsync<ModelException>(() => genreController.CreateAsync(genre));

            _mockGenreService.Verify(x => x.CreateAsync(genre), Times.Once);
        }

        [Test]
        public async Task EditAsync_ValidGenreModel_ExecOnce()
        {
            _mockGenreService.Setup(x => x.EditAsync(It.IsAny<GenreModel>())).ReturnsAsync(() => new GenreModel());
            GenreController genreController = new GenreController(_mockGenreService.Object);
            GenreModel genre = new GenreModel();

            await genreController.EditAsync(genre);

            _mockGenreService.Verify(x => x.EditAsync(genre), Times.Once);
        }

        [Test]
        public void EditAsync_NotUniqueKey_ThrowModelException()
        {
            _mockGenreService.Setup(x => x.EditAsync(It.IsAny<GenreModel>()))
                .Throws(() => new ModelException());

            GenreController genreController = new GenreController(_mockGenreService.Object);
            GenreModel genre = new GenreModel();

            Assert.ThrowsAsync<ModelException>(() => genreController.EditAsync(genre));

            _mockGenreService.Verify(x => x.EditAsync(genre), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ValidGenreModel_ExecOnce()
        {
            _mockGenreService.Setup(x => x.DeleteAsync(It.IsAny<Guid>()));
            GenreController genreController = new GenreController(_mockGenreService.Object);
            Guid genreId = Guid.NewGuid();

            await genreController.DeleteAsync(genreId);

            _mockGenreService.Verify(x => x.DeleteAsync(genreId), Times.Once);
        }

        [Test]
        public void DeleteAsync_NotUniqueKey_ThrowModelException()
        {
            _mockGenreService.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .Throws(() => new ModelException());

            GenreController genreController = new GenreController(_mockGenreService.Object);
            Guid genreId = Guid.NewGuid();

            Assert.ThrowsAsync<ModelException>(() => genreController.DeleteAsync(genreId));

            _mockGenreService.Verify(x => x.DeleteAsync(genreId), Times.Once);
        }
    }
}

