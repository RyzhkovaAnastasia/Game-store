using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Common.Enums;
using System;
using System.Threading.Tasks;

namespace OnlineGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformTypeController : ControllerBase
    {
        private readonly IPlatformTypeService _platformTypeService;
        public PlatformTypeController(IPlatformTypeService platformTypeService)
        {
            _platformTypeService = platformTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var platforms = await _platformTypeService.GetAllAsync();
            return Ok(platforms);
        }

        [HttpGet("{id}")]
        [RoleAuthorize(Role.Administrator, Role.Manager)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var platform = await _platformTypeService.GetByIdAsync(id);
            return Ok(platform);
        }


        [HttpPost]
        [RoleAuthorize(Role.Administrator, Role.Manager)]
        public async Task<IActionResult> CreateAsync(PlatformTypeModel newPlatformType)
        {
            var platformType = await _platformTypeService.CreateAsync(newPlatformType);
            return Ok(platformType);
        }

        [HttpPut]
        [RoleAuthorize(Role.Administrator, Role.Manager)]
        public async Task<IActionResult> EditAsync(PlatformTypeModel updatedPlatformType)
        {
            var platformType = await _platformTypeService.EditAsync(updatedPlatformType);
            return Ok(platformType);
        }

        [HttpDelete("{id}")]
        [RoleAuthorize(Role.Administrator, Role.Manager)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _platformTypeService.DeleteAsync(id);
            return Ok();
        }
    }
}
