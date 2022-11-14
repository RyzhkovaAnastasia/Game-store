using Moq;
using NUnit.Framework;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models.AuthModels;
using OnlineGameStore.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.API.UnitTests
{
    public class RoleControllerTests
    {
        private Mock<IRoleService> _mockRoleService;

        [SetUp]
        public void Setup()
        {
            _mockRoleService = new Mock<IRoleService>();
        }

        [Test]
        public void GetAllAsyncRoles_ExecOnce()
        {
            _mockRoleService.Setup(x => x.GetAll()).Returns(() => new List<RoleModel>());
            RoleController roleController = new RoleController(_mockRoleService.Object);

            var actual = roleController.GetAll();

            _mockRoleService.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ValidKey_ExecOnce()
        {
            _mockRoleService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => new RoleModel());
            RoleController roleController = new RoleController(_mockRoleService.Object);
            Guid roleId = Guid.NewGuid();

            await roleController.GetByIdAsync(roleId);

            _mockRoleService.Verify(x => x.GetByIdAsync(roleId), Times.Once);
        }
    }
}
