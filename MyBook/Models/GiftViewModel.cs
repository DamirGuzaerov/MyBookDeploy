using MyBook.Entities;
using System.ComponentModel.DataAnnotations;
using Type = MyBook.Entities.Type;

namespace MyBook.Models
{
    public class GiftViewModel
    {
        const string email = @"^(?![\w\.@]*\.\.)(?![\w\.@]*\.@)(?![\w\.]*@\.)\w+[\w\.]*@[\w\.]+\.\w{2,}$";

        [Required(ErrorMessage = "Введите email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [RegularExpression(email,ErrorMessage ="Введите корректный email")]
        public string Email { get; set; }

        public List<Type> SubscrTypes { get; set; } = new List<Type>();
        public List<Author> Authors { get; set; } = new List<Author>();
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public string? GenreName { get; set; }
        public string? AuthorName { get; set; }
        [Required(ErrorMessage = "Выберите тип подписки")]
        public int? TypeId { get; set; } = null;
    }
}
