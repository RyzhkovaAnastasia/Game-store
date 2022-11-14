using OnlineGameStore.BLL.Models;
using System;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface IOrderDetailService
    {
        Task<OrderDetailModel> AddToBasketAsync(OrderDetailModel newItem, Guid customerId);
        Task RemoveFromBasketAsync(Guid id);
    }
}
