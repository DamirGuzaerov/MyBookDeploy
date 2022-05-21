using System.ComponentModel.DataAnnotations.Schema;

namespace MyBook.Entities
{
    public partial class Rating
    {
        public int RatingId { get; set; }
        public string ReviewText { get; set; }
        public double Points { get; set; }
        [Column("UserId")]
        public string UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public int BookId { get; set; }
        public virtual Book Book { get; set; } = null!;
    }
}
