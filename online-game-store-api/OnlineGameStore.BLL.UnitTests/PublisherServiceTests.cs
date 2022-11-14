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
    public class PublisherServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<Publisher>> _mockPublisherRepository;
        private readonly IMapper _mapper;
        public PublisherServiceTests()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mc => new MappingProfile(mc));
            _mapper = mapperConfiguration.CreateMapper();
        }

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPublisherRepository = new Mock<IRepository<Publisher>>();
            _mockUnitOfWork.Setup(x => x.PublisherRepository).Returns(_mockPublisherRepository.Object);
        }

        [Test]
        public async Task GetAll_ReturnPublisherList()
        {

            _mockPublisherRepository
                .Setup(x => x.GetAllAsync(null))
                .ReturnsAsync(() => new List<Publisher>());

            PublisherService publisherService = new PublisherService(_mockUnitOfWork.Object, _mapper);


            IEnumerable<PublisherModel> publishers = await publisherService.GetAllAsync();


            Assert.IsNotNull(publishers);
            _mockUnitOfWork.Verify(x => x.PublisherRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PublisherRepository.GetAllAsync(null), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnPublisher()
        {

            _mockPublisherRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
                .ReturnsAsync(() => new Publisher());

            PublisherService publisherService = new PublisherService(_mockUnitOfWork.Object, _mapper);


            PublisherModel publisher = await publisherService.GetByIdAsync(Guid.NewGuid());


            Assert.IsNotNull(publisher);
            _mockUnitOfWork.Verify(x => x.PublisherRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PublisherRepository.FindAsync(It.IsAny<Expression<Func<Publisher, bool>>>()), Times.Once);
        }

        [Test]
        public void GetByIdAsync_NotExistingId_ThrowNotFoundException()
        {

            _mockPublisherRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
                .ReturnsAsync(() => null);

            PublisherService publisherService = new PublisherService(_mockUnitOfWork.Object, _mapper);



            Assert.ThrowsAsync<NotFoundException>(() => publisherService.GetByIdAsync(Guid.NewGuid()));
            _mockUnitOfWork.Verify(x => x.PublisherRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PublisherRepository.FindAsync(It.IsAny<Expression<Func<Publisher, bool>>>()), Times.Once);
        }


        [Test]
        public async Task EditAsync_ValidPublisher_ReturnNewPublisherModel()
        {

            _mockPublisherRepository
                .Setup(x => x.EditAsync(It.IsAny<Publisher>()))
                .ReturnsAsync(() => new Publisher());

            _mockPublisherRepository
               .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
               .ReturnsAsync(() => new List<Publisher>());

            PublisherService publisherService = new PublisherService(_mockUnitOfWork.Object, _mapper);
            PublisherModel editedPublisherModel = new PublisherModel();


            PublisherModel publishers = await publisherService.EditAsync(editedPublisherModel);


            Assert.IsNotNull(publishers);
            _mockUnitOfWork.Verify(x => x.PublisherRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PublisherRepository.EditAsync(It.IsAny<Publisher>()), Times.Once);
        }

        [Test]
        public void EditAsync_InvalidPublisherId_ThrowException()
        {

            _mockPublisherRepository
                .Setup(x => x.EditAsync(It.IsAny<Publisher>()))
                .ReturnsAsync(() => null);

            _mockPublisherRepository
               .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
               .ReturnsAsync(() => new List<Publisher>());

            PublisherService publisherService = new PublisherService(_mockUnitOfWork.Object, _mapper);
            PublisherModel editedPublisherModel = new PublisherModel();



            Assert.ThrowsAsync<NotFoundException>(() => publisherService.EditAsync(editedPublisherModel));
            _mockUnitOfWork.Verify(x => x.PublisherRepository.EditAsync(It.IsAny<Publisher>()), Times.Once);
        }

        [Test]
        public void Editsync_InvalidPublisher_ThrowException()
        {

            _mockPublisherRepository
                .Setup(x => x.EditAsync(It.IsAny<Publisher>()))
                .Throws(new Exception());

            _mockPublisherRepository
               .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
               .ReturnsAsync(() => new List<Publisher>());

            PublisherService publisherService = new PublisherService(_mockUnitOfWork.Object, _mapper);
            PublisherModel updatedPublisherModel = new PublisherModel();


            Assert.ThrowsAsync<Exception>(() => publisherService.EditAsync(updatedPublisherModel));

            _mockUnitOfWork.Verify(x => x.PublisherRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PublisherRepository.EditAsync(It.IsAny<Publisher>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidPublisher_ReturnNewPublisherModel()
        {

            _mockPublisherRepository
                .Setup(x => x.CreateAsync(It.IsAny<Publisher>()))
                .ReturnsAsync(() => new Publisher());

            _mockPublisherRepository
               .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
               .ReturnsAsync(() => new List<Publisher>());

            PublisherService publisherService = new PublisherService(_mockUnitOfWork.Object, _mapper);
            PublisherModel newPublisherModel = new PublisherModel();


            PublisherModel publishers = await publisherService.CreateAsync(newPublisherModel);


            Assert.IsNotNull(publishers);
            _mockUnitOfWork.Verify(x => x.PublisherRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PublisherRepository.CreateAsync(It.IsAny<Publisher>()), Times.Once);
        }

        [Test]
        public void CreateAsync_InvalidPublisher_ThrowException()
        {

            _mockPublisherRepository
                .Setup(x => x.CreateAsync(It.IsAny<Publisher>()))
                .Throws(new Exception());


            _mockPublisherRepository
               .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
               .ReturnsAsync(() => new List<Publisher>());

            PublisherService publisherService = new PublisherService(_mockUnitOfWork.Object, _mapper);
            PublisherModel newPublisherModel = new PublisherModel();


            Assert.ThrowsAsync<Exception>(() => publisherService.CreateAsync(newPublisherModel));

            _mockUnitOfWork.Verify(x => x.PublisherRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PublisherRepository.CreateAsync(It.IsAny<Publisher>()), Times.Once);
        }


        [Test]
        public async Task DeleteAsync_ValidPublisher_DeletePublisher()
        {

            _mockPublisherRepository
                .Setup(x => x.SoftDeleteAsync(It.IsAny<Guid>()));

            PublisherService publisherService = new PublisherService(_mockUnitOfWork.Object, _mapper);


            await publisherService.DeleteAsync(Guid.NewGuid());


            _mockUnitOfWork.Verify(x => x.PublisherRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PublisherRepository.SoftDeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void DeleteAsync_InvalidPublisher_ThrowException()
        {

            _mockPublisherRepository
                .Setup(x => x.SoftDeleteAsync(It.IsAny<Guid>()))
                .Throws(new Exception());

            PublisherService publisherService = new PublisherService(_mockUnitOfWork.Object, _mapper);


            Assert.ThrowsAsync<Exception>(() => publisherService.DeleteAsync(Guid.NewGuid()));

            _mockUnitOfWork.Verify(x => x.PublisherRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PublisherRepository.SoftDeleteAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
