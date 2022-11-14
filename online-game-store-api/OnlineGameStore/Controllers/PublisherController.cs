using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Common.Enums;
using OnlineGameStore.Extensions;
using System;
using System.Threading.Tasks;

namespace OnlineGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private readonly UserClaims _userClaims;

        public PublisherController(IPublisherService publisherService, UserClaims userClaims)
        {
            _publisherService = publisherService;
            _userClaims = userClaims;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var publishers = await _publisherService.GetAllAsync();
            return Ok(publishers);
        }

        [HttpGet("{companyName}")]
        public async Task<IActionResult> GetByCompanyNameAsync(string companyName)
        {
            PublisherModel publisher = await _publisherService.GetByCompanyNameAsync(companyName);
            return Ok(publisher);
        }

        [HttpPost]
        [RoleAuthorize(Role.Administrator, Role.Manager)]
        public async Task<IActionResult> CreateAsync(PublisherModel newPublisher)
        {
            PublisherModel publisher = await _publisherService.CreateAsync(newPublisher);
            return Ok(publisher);
        }

        [HttpPut]
        [RoleAuthorize(Role.Administrator, Role.Manager, Role.Publisher)]
        public async Task<IActionResult> EditAsync(PublisherModel updatedPublisher)
        {
            if (_userClaims.IsUserHasAccess(updatedPublisher.Id))
            {
                PublisherModel publisher = await _publisherService.EditAsync(updatedPublisher);
                return Ok(publisher);
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        [RoleAuthorize(Role.Administrator, Role.Manager)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _publisherService.DeleteAsync(id);
            return Ok();
        }
    }
}
