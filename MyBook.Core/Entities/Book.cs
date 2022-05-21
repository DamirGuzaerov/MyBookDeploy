namespace MyBook.Entities
{
    public partial class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string? PublishedDate { get; set; }
        public bool IsForAdult { get; set; }
        public bool IsPaid { get; set; }

        public int DownloadsCount { get; set; } = 0;
        public int BookDescId { get; set; }
        public virtual BookDesc Description { get; set; } = null!;
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();
        public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        public virtual ICollection<History> Histories { get; set; }=new List<History>();
        public virtual ICollection<ImgLink> ImgLinks { get; set; }= new List<ImgLink>();
    }
}
