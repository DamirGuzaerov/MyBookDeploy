namespace MyBook.Entities
{
    public partial class AuthorBook
    {
        public int AuthorBookId { get; set; }
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; } = null!;
    
        public int BookId { get; set; }
        public virtual Book Book { get; set; } = null!;
    }
}
