using MyBook.Entities;

namespace MyBook.Models
{
    public class AuthorViewModel
    {
        public string Name { get; set; } = null!;
        public String? BirthDate { get; set; }

        public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();
        public virtual ICollection<ImgLink> ImgLinks { get; set; } = new List<ImgLink>();
        public bool HasSubscr { get; set; } = false;
    }
}
