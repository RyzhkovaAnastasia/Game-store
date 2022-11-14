using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Extensions;
using System;
using System.Threading.Tasks;

namespace OnlineGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly UserClaims _userClaims;

        public OrderDetailController(IOrderDetailService orderDetailService, UserClaims userClaims)
        {
            _orderDetailService = orderDetailService;
            _userClaims = userClaims;
        }

        [HttpPost]
        public async Task<IActionResult> AddBasketItem(OrderDetailModel orderDetailModel)
        {
            await _orderDetailService.AddToBasketAsync(orderDetailModel, _userClaims.Id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketItem(Guid id)
        {
            await _orderDetailService.RemoveFromBasketAsync(id);
            return Ok();
        }
    }
}
