using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using OnlineGameStore.Controllers;

namespace OnlineGameStore.API.UnitTests
{
    public class ResourceControllerTests
    {
        private Mock<IWebHostEnvironment> _mockEnvironment;

        [SetUp]
        public void Setup()
        {
            _mockEnvironment = new Mock<IWebHostEnvironment>();
        }


        [Test]
        public void GetImages_ValidKey_ExecOnce()
        {
            _mockEnvironment.Setup(x => x.ContentRootPath).Returns("example");
            ResourceController resController = new ResourceController(_mockEnvironment.Object);

            var actual = resController.GetImage("");
            Assert.NotNull(actual);
        }
    }
}
