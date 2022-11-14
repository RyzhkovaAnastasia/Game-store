using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.BLL.Services
{
    public class VisaPaymentService : IPaymentStrategy
    {
        public bool Pay(object paymentInfo)
        {
            return true;
        }
    }
}
