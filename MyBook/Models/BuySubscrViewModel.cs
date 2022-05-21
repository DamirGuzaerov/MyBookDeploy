using MyBook.Entities;

namespace MyBook.Models
{
    public class BuySubscrViewModel
    {
        public List<MyBook.Entities.Type> SubscrTypes { get; set; } = new();
        public List<Genre> Genres { get; set; } = new();
        public List<Author> Authors { get; set; } = new();

        public string GenreName { get; set; }
        public string AuthorName { get; set; }
        public MyBook.Entities.Type Type { get; set; }
    }
}
