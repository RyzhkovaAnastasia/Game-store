using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.BLL.Services
{
    public class BankPaymentService : IPaymentStrategy
    {

        public bool Pay(object paymentInfo)
        {
            return true;
        }
    }
}
