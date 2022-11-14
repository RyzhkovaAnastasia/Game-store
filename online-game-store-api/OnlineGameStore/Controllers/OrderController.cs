using Microsoft.AspNetCore.Authorization;
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
    [RoleAuthorize(Role.Administrator, Role.Manager)]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserClaims _userClaims;
        public OrderController(IOrderService orderService, UserClaims userClaims)
        {
            _orderService = orderService;
            _userClaims = userClaims;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveOrder()
        {
            var order = await _orderService.GetLastOrderByUserIdAsync(_userClaims.Id);
            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return Ok(order);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetOrders([FromQuery] DateQueryModel date)
        {
            var orders = await _orderService.GetAllAsync(date.StartDate ?? DateTime.MinValue, date.EndDate ?? DateTime.MaxValue);
            return Ok(orders);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> EditOrder(OrderModel updatedOrder)
        {
            OrderModel order = await _orderService.EditAsync(updatedOrder);
            return Ok(order);
        }
    }
}
