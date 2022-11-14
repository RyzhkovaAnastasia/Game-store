using System;

namespace OnlineGameStore.BLL.Models
{
    public class PaymentMethodModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageFileName { get; set; }
    }
}
