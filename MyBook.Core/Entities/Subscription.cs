using System.ComponentModel.DataAnnotations.Schema;

namespace MyBook.Entities
{
    public partial class Subscription
    {
        public int SubscriptionId { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public int TypeId { get; set; }

        public virtual Type Type { get; set; }
        public virtual UserSubscr UserSubscr { get; set; } = null!;
        public int? AuthorId { get; set; }
        public virtual Author? Author { get; set; }
        public int? GenreId { get; set; }
        public virtual Genre? Genre { get; set; }
    }
}