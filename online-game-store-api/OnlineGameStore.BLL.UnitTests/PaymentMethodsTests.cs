using NUnit.Framework;
using OnlineGameStore.BLL.Services;

namespace OnlineGameStore.BLL.UnitTests
{
    internal class PaymentMethodsTests
    {
        [Test]
        public void IboxPaymentProcess_ReturnTrue()
        {
            var ibox = new IBoxPaymentService();

            var actual = ibox.Pay(new { });

            Assert.IsTrue(actual);
        }

        [Test]
        public void VisaPaymentProcess_ReturnTrue()
        {
            var visa = new VisaPaymentService();

            var actual = visa.Pay(new { });

            Assert.IsTrue(actual);
        }

        [Test]
        public void BankPaymentProcess_ReturnTrue()
        {
            var bank = new BankPaymentService();

            var actual = bank.Pay(new { });

            Assert.IsTrue(actual);
        }
    }
}
