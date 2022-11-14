using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.Common.Enums;
using System;
using System.Threading.Tasks;

namespace OnlineGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RoleAuthorize(Role.Administrator, Role.Manager)]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;
        public ShipperController(IShipperService shipperService)
        {
            _shipperService = shipperService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var shippers = await _shipperService.GetAllAsync();
            return Ok(shippers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var shipper = await _shipperService.GetByIdAsync(id);
            return Ok(shipper);
        }
    }
}
