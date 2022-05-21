namespace MyBook.Entities
{
    public partial class History
    {
        public int HistoryId { get; set; }
        public string DateTime { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; } = null!;
        public string UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
