using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.Common.Enums;
using System;
using System.Threading.Tasks;

namespace OnlineGameStore.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [RoleAuthorize(Role.Administrator)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = _roleService.GetAll();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);
            return Ok(role);
        }
    }

}
