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
    public class GenreServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<Genre>> _mockGenreRepository;
        private readonly IMapper _mapper;
        public GenreServiceTests()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mc => new MappingProfile(mc));
            _mapper = mapperConfiguration.CreateMapper();
        }

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockGenreRepository = new Mock<IRepository<Genre>>();
            _mockUnitOfWork.Setup(x => x.GenreRepository).Returns(_mockGenreRepository.Object);
        }

        [Test]
        public async Task GetAll_ReturnGenreList()
        {

            _mockGenreRepository
                .Setup(x => x.GetAllAsync(null))
                .ReturnsAsync(() => new List<Genre>());

            GenreService genreService = new GenreService(_mockUnitOfWork.Object, _mapper);


            IEnumerable<GenreModel> genres = await genreService.GetAllAsync();


            Assert.IsNotNull(genres);
            _mockUnitOfWork.Verify(x => x.GenreRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GenreRepository.GetAllAsync(null), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnGenre()
        {

            _mockGenreRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .ReturnsAsync(() => new Genre());

            GenreService genreService = new GenreService(_mockUnitOfWork.Object, _mapper);


            GenreModel genre = await genreService.GetByIdAsync(Guid.NewGuid());


            Assert.IsNotNull(genre);
            _mockUnitOfWork.Verify(x => x.GenreRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GenreRepository.FindAsync(It.IsAny<Expression<Func<Genre, bool>>>()), Times.Once);
        }

        [Test]
        public void GetByIdAsync_NotExistingId_ThrowNotFoundException()
        {

            _mockGenreRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .ReturnsAsync(() => null);

            GenreService genreService = new GenreService(_mockUnitOfWork.Object, _mapper);



            Assert.ThrowsAsync<NotFoundException>(() => genreService.GetByIdAsync(Guid.NewGuid()));
            _mockUnitOfWork.Verify(x => x.GenreRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GenreRepository.FindAsync(It.IsAny<Expression<Func<Genre, bool>>>()), Times.Once);
        }


        [Test]
        public async Task EditAsync_ValidGenre_ReturnNewGenreModel()
        {

            _mockGenreRepository
                .Setup(x => x.EditAsync(It.IsAny<Genre>()))
                .ReturnsAsync(() => new Genre());

            _mockGenreRepository
               .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
               .ReturnsAsync(() => new List<Genre>());


            GenreService genreService = new GenreService(_mockUnitOfWork.Object, _mapper);
            GenreModel editedGenreModel = new GenreModel();


            GenreModel genres = await genreService.EditAsync(editedGenreModel);


            Assert.IsNotNull(genres);
            _mockUnitOfWork.Verify(x => x.GenreRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GenreRepository.EditAsync(It.IsAny<Genre>()), Times.Once);
        }

        [Test]
        public void EditAsync_InvalidGenreId_ThrowException()
        {

            _mockGenreRepository
                .Setup(x => x.EditAsync(It.IsAny<Genre>()))
                .ReturnsAsync(() => null);

            _mockGenreRepository
              .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
              .ReturnsAsync(() => new List<Genre>());

            GenreService genreService = new GenreService(_mockUnitOfWork.Object, _mapper);
            GenreModel editedGenreModel = new GenreModel();



            Assert.ThrowsAsync<NotFoundException>(() => genreService.EditAsync(editedGenreModel));
            _mockUnitOfWork.Verify(x => x.GenreRepository.EditAsync(It.IsAny<Genre>()), Times.Once);
        }

        [Test]
        public void Editsync_InvalidGenre_ThrowException()
        {

            _mockGenreRepository
                .Setup(x => x.EditAsync(It.IsAny<Genre>()))
                .Throws(new Exception());

            _mockGenreRepository
              .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
              .ReturnsAsync(() => new List<Genre>() { new Genre() { } });

            GenreService genreService = new GenreService(_mockUnitOfWork.Object, _mapper);
            GenreModel updatedGenreModel = new GenreModel();


            Assert.ThrowsAsync<Exception>(() => genreService.EditAsync(updatedGenreModel));

            _mockUnitOfWork.Verify(x => x.GenreRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GenreRepository.EditAsync(It.IsAny<Genre>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidGenre_ReturnNewGenreModel()
        {

            _mockGenreRepository
                .Setup(x => x.CreateAsync(It.IsAny<Genre>()))
                .ReturnsAsync(() => new Genre());

            _mockGenreRepository
             .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
             .ReturnsAsync(() => new List<Genre>() { });


            GenreService genreService = new GenreService(_mockUnitOfWork.Object, _mapper);
            GenreModel newGenreModel = new GenreModel();


            GenreModel genres = await genreService.CreateAsync(newGenreModel);


            Assert.IsNotNull(genres);
            _mockUnitOfWork.Verify(x => x.GenreRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GenreRepository.CreateAsync(It.IsAny<Genre>()), Times.Once);
        }

        [Test]
        public void CreateAsync_InvalidGenre_ThrowException()
        {
            _mockGenreRepository
                .Setup(x => x.CreateAsync(It.IsAny<Genre>()))
                .Throws(new Exception());

            _mockGenreRepository
             .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
             .ReturnsAsync(() => new List<Genre>() { new Genre() { } });

            GenreService genreService = new GenreService(_mockUnitOfWork.Object, _mapper);
            GenreModel newGenreModel = new GenreModel();


            Assert.ThrowsAsync<Exception>(() => genreService.CreateAsync(newGenreModel));

            _mockUnitOfWork.Verify(x => x.GenreRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GenreRepository.CreateAsync(It.IsAny<Genre>()), Times.Once);
        }


        [Test]
        public async Task DeleteAsync_ValidGenre_DeleteGenre()
        {

            _mockGenreRepository
                .Setup(x => x.SoftDeleteAsync(It.IsAny<Guid>()));
            _mockGenreRepository
               .Setup(x => x.EditAsync(It.IsAny<Genre>()));
            _mockGenreRepository
               .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Genre, bool>>>()));

            GenreService genreService = new GenreService(_mockUnitOfWork.Object, _mapper);


            await genreService.DeleteAsync(Guid.NewGuid());


            _mockUnitOfWork.Verify(x => x.GenreRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GenreRepository.SoftDeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void DeleteAsync_InvalidGenre_ThrowException()
        {

            _mockGenreRepository
                .Setup(x => x.SoftDeleteAsync(It.IsAny<Guid>()))
                .Throws(new Exception());

            GenreService genreService = new GenreService(_mockUnitOfWork.Object, _mapper);


            Assert.ThrowsAsync<Exception>(() => genreService.DeleteAsync(Guid.NewGuid()));

            _mockUnitOfWork.Verify(x => x.GenreRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.GenreRepository.SoftDeleteAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
