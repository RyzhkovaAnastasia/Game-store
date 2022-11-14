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
    public class PlatformTypeServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<PlatformType>> _mockPlatformTypeRepository;
        private readonly IMapper _mapper;
        public PlatformTypeServiceTests()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mc => new MappingProfile(mc));
            _mapper = mapperConfiguration.CreateMapper();
        }

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPlatformTypeRepository = new Mock<IRepository<PlatformType>>();
            _mockUnitOfWork.Setup(x => x.PlatformTypeRepository).Returns(_mockPlatformTypeRepository.Object);
        }

        [Test]
        public async Task GetAll_ReturnPlatformTypeList()
        {

            _mockPlatformTypeRepository
                .Setup(x => x.GetAllAsync(null))
                .ReturnsAsync(() => new List<PlatformType>());

            PlatformTypeService platformTypeService = new PlatformTypeService(_mockUnitOfWork.Object, _mapper);


            IEnumerable<PlatformTypeModel> platformTypes = await platformTypeService.GetAllAsync();


            Assert.IsNotNull(platformTypes);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository.GetAllAsync(null), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnPlatformType()
        {

            _mockPlatformTypeRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<PlatformType, bool>>>()))
                .ReturnsAsync(() => new PlatformType());

            PlatformTypeService platformTypeService = new PlatformTypeService(_mockUnitOfWork.Object, _mapper);


            PlatformTypeModel platformType = await platformTypeService.GetByIdAsync(Guid.NewGuid());


            Assert.IsNotNull(platformType);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository.FindAsync(It.IsAny<Expression<Func<PlatformType, bool>>>()), Times.Once);
        }

        [Test]
        public void GetByIdAsync_NotExistingId_ThrowNotFoundException()
        {

            _mockPlatformTypeRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<PlatformType, bool>>>()))
                .ReturnsAsync(() => null);

            PlatformTypeService platformTypeService = new PlatformTypeService(_mockUnitOfWork.Object, _mapper);



            Assert.ThrowsAsync<NotFoundException>(() => platformTypeService.GetByIdAsync(Guid.NewGuid()));
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository.FindAsync(It.IsAny<Expression<Func<PlatformType, bool>>>()), Times.Once);
        }


        [Test]
        public async Task EditAsync_ValidPlatformType_ReturnNewPlatformTypeModel()
        {

            _mockPlatformTypeRepository
                .Setup(x => x.EditAsync(It.IsAny<PlatformType>()))
                .ReturnsAsync(() => new PlatformType());

            PlatformTypeService platformTypeService = new PlatformTypeService(_mockUnitOfWork.Object, _mapper);
            PlatformTypeModel editedPlatformTypeModel = new PlatformTypeModel();


            PlatformTypeModel platformTypes = await platformTypeService.EditAsync(editedPlatformTypeModel);


            Assert.IsNotNull(platformTypes);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository.EditAsync(It.IsAny<PlatformType>()), Times.Once);
        }

        [Test]
        public void EditAsync_InvalidPlatformTypeId_ThrowException()
        {

            _mockPlatformTypeRepository
                .Setup(x => x.EditAsync(It.IsAny<PlatformType>()))
                .ReturnsAsync(() => null);

            PlatformTypeService platformTypeService = new PlatformTypeService(_mockUnitOfWork.Object, _mapper);
            PlatformTypeModel editedPlatformTypeModel = new PlatformTypeModel();



            Assert.ThrowsAsync<NotFoundException>(() => platformTypeService.EditAsync(editedPlatformTypeModel));
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository.EditAsync(It.IsAny<PlatformType>()), Times.Once);
        }

        [Test]
        public void Editsync_InvalidPlatformType_ThrowException()
        {

            _mockPlatformTypeRepository
                .Setup(x => x.EditAsync(It.IsAny<PlatformType>()))
                .Throws(new Exception());

            PlatformTypeService platformTypeService = new PlatformTypeService(_mockUnitOfWork.Object, _mapper);
            PlatformTypeModel updatedPlatformTypeModel = new PlatformTypeModel();


            Assert.ThrowsAsync<Exception>(() => platformTypeService.EditAsync(updatedPlatformTypeModel));

            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository.EditAsync(It.IsAny<PlatformType>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidPlatformType_ReturnNewPlatformTypeModel()
        {

            _mockPlatformTypeRepository
                .Setup(x => x.CreateAsync(It.IsAny<PlatformType>()))
                .ReturnsAsync(() => new PlatformType());

            PlatformTypeService platformTypeService = new PlatformTypeService(_mockUnitOfWork.Object, _mapper);
            PlatformTypeModel newPlatformTypeModel = new PlatformTypeModel();


            PlatformTypeModel platformTypes = await platformTypeService.CreateAsync(newPlatformTypeModel);


            Assert.IsNotNull(platformTypes);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository.CreateAsync(It.IsAny<PlatformType>()), Times.Once);
        }

        [Test]
        public void CreateAsync_InvalidPlatformType_ThrowException()
        {

            _mockPlatformTypeRepository
                .Setup(x => x.CreateAsync(It.IsAny<PlatformType>()))
                .Throws(new Exception());

            PlatformTypeService platformTypeService = new PlatformTypeService(_mockUnitOfWork.Object, _mapper);
            PlatformTypeModel newPlatformTypeModel = new PlatformTypeModel();


            Assert.ThrowsAsync<Exception>(() => platformTypeService.CreateAsync(newPlatformTypeModel));

            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository.CreateAsync(It.IsAny<PlatformType>()), Times.Once);
        }


        [Test]
        public async Task DeleteAsync_ValidPlatformType_DeletePlatformType()
        {

            _mockPlatformTypeRepository
                .Setup(x => x.SoftDeleteAsync(It.IsAny<Guid>()));

            PlatformTypeService platformTypeService = new PlatformTypeService(_mockUnitOfWork.Object, _mapper);


            await platformTypeService.DeleteAsync(Guid.NewGuid());


            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository.SoftDeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void DeleteAsync_InvalidPlatformType_ThrowException()
        {

            _mockPlatformTypeRepository
                .Setup(x => x.SoftDeleteAsync(It.IsAny<Guid>()))
                .Throws(new Exception());

            PlatformTypeService platformTypeService = new PlatformTypeService(_mockUnitOfWork.Object, _mapper);


            Assert.ThrowsAsync<Exception>(() => platformTypeService.DeleteAsync(Guid.NewGuid()));

            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository, Times.AtLeastOnce);
            _mockUnitOfWork.Verify(x => x.PlatformTypeRepository.SoftDeleteAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
