namespace OnlineGameStore.DAL.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageFileName { get; set; }
    }
}
