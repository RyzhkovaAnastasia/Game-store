using OnlineGameStore.BLL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodModel>> GetAllAsync();
        public void SetPaymentMethod(IPaymentStrategy paymentStrategy);
        public Task PayAsync(Guid userId, object paymentInfo);
        Task StartTimeout(Guid customerId, int timeoutMinutes);
    }
}
