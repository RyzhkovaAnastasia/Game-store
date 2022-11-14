using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.BLL.Services
{
    public class IBoxPaymentService : IPaymentStrategy
    {
        public bool Pay(object paymentInfo)
        {
            return true;
        }
    }
}
