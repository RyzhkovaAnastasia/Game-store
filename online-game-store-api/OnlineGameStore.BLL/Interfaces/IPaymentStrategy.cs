namespace OnlineGameStore.BLL.Interfaces
{
    public interface IPaymentStrategy
    {
        bool Pay(object paymentInfo);
    }
}
