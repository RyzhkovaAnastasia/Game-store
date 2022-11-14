using OnlineGameStore.BLL.Models;
using OnlineGameStore.Common.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetAllAsync();
        Task<IEnumerable<OrderModel>> GetAllAsync(DateTime startDate, DateTime endDate);
        Task<OrderModel> GetLastOrderByUserIdAsync(Guid userId);
        Task<OrderModel> GetByIdAsync(Guid id);
        Task<OrderModel> CreateAsync(OrderModel newOrder);
        Task<OrderModel> EditAsync(OrderModel updatedOrder);
        Task DeleteAsync(Guid id);

        Task TakeAwaySoldGames(IEnumerable<OrderDetailModel> orderDetails);
        Task ReturnGamesInStock(IEnumerable<OrderDetailModel> orderDetails);
        Task ManageOrderAfterTimeout(OrderModel order);
        Task<OrderModel> ChangeOrderState(OrderModel order, OrderState orderState);
        Task MergeBasketsAsync(Guid guestBasketId, Guid userBasketId);
    }
}
