namespace MyBook.Entities
{
    public class Type
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
