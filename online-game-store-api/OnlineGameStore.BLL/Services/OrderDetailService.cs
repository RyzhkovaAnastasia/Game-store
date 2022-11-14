using AutoMapper;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderDetailService(IUnitOfWork unitOfWork, IOrderService orderService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderService = orderService;
        }

        public async Task<OrderDetailModel> AddToBasketAsync(OrderDetailModel newItem, Guid customerId)
        {
            var order = await _orderService.GetLastOrderByUserIdAsync(customerId);
            if (order == null)
            {
                order = await _orderService.CreateAsync(new OrderModel() { UserId = customerId });
            }

            newItem.OrderId = order.Id;

            var existingOrderItem = await _unitOfWork.OrderDetailRepository.FindAsync(x => x.OrderId == newItem.OrderId && x.GameId == newItem.GameId);
            if (existingOrderItem != null)
            {
                existingOrderItem.Quantity += newItem.Quantity;
                await _unitOfWork.OrderDetailRepository.EditAsync(existingOrderItem);
                return _mapper.Map<OrderDetailModel>(existingOrderItem);
            }

            OrderDetail orderItemEntity = _mapper.Map<OrderDetail>(newItem);
            await _unitOfWork.OrderDetailRepository.CreateAsync(orderItemEntity);
            return newItem;
        }

        public async Task RemoveFromBasketAsync(Guid id)
        {
            await _unitOfWork.OrderDetailRepository.DeleteAsync(id);
        }
    }
}
