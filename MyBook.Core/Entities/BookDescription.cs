namespace MyBook.Entities
{
    public partial class BookDesc
    {
        public int BookDescId { get; set; }
        public string? Description { get; set; }
        public int PagesCount { get; set; }
        public string? Price { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; } = null!;
        public virtual ICollection<DownloadLink> DownloadLinks { get; set; } = new List<DownloadLink>();
    }
}

