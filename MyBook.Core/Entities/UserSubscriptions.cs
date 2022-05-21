namespace MyBook.Entities
{
    public partial class UserSubscr
    {
        public int UserSubscrId { get; set; }
        public int SubscriptionId { get; set; }
        public virtual Subscription Subscription { get; set; } = null!;
        public string UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
