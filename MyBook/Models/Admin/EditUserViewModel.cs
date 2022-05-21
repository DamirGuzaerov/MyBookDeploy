using MyBook.Entities;
using System.ComponentModel.DataAnnotations;
using Type = MyBook.Entities.Type;

namespace MyBook.Models.Admin
{
    public class EditUserViewModel
    {
        public List<Type> SubscrTypes { get; set; } = new List<Type>();
        public List<Author> Authors { get; set; } = new List<Author>();
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public string UserID { get; set; }
        public string? GenreName { get; set; }
        public string? AuthorName { get; set; }
        [Required(ErrorMessage = "Выберите период")]
        public int? Period { get; set; } = null;
        [Required(ErrorMessage = "Выберите тип подписки")]
        public int? TypeId { get; set; } = null;
    }
}
