using MyBook.Core.Interfaces;

namespace MyBook.Services
{
    public class PaymentService : IPaymentService
    {
        private int counter = 0;
        public bool PerformPayment()
        {
            counter++;
            return counter % 4 != 0;
        }
    }
}
