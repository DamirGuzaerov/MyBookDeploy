namespace MyBook.Entities
{
    public partial class ImgLink
    {
        public int ImgLinkId { get; set; }
        public string? Resolution { get; set; }
        public string Url { get; set; } = null!;
        public int? AuthorId { get; set; }
        
        public virtual Author? Author { get; set; }
        public int? BookId { get; set; }
        public virtual Book? Book { get; set; }
    }
}
