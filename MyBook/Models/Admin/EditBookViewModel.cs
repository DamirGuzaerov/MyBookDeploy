using MyBook.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBook.Models.Admin
{
    public class EditBookViewModel
    {
        const string text = @"^([А-Я]|[а-яё]|[A-z]|[A-z]|\s|\d|[.,!?:;-])*";
        const string price = @"^((\d{1,3}|\s*){1})((\,\d{3}|\d)*)(\s*|\.(\d{2}))$";
        const string url = @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$";
        public int? BookId { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [MaxLength(50, ErrorMessage = "Длина названия не должна превышать 50 символов"), MinLength(5, ErrorMessage = "Длина названия не должна быть меньше 5 символов")]
        [RegularExpression(text, ErrorMessage = "Введите корректное название", MatchTimeoutInMilliseconds = 10)]
        [DataType(DataType.Text)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Выберите язык")]
        public string Language { get; set; } = null!;
        [DataType(DataType.Date, ErrorMessage = "Введите корректную дату рождения")]
        public string? PublishedDate { get; set; }
        [Required(ErrorMessage = "Выберите является ли книга 18+")]
        public bool IsForAdult { get; set; }
        [Required(ErrorMessage = "Выберите платная ли книга")]
        public bool IsPaid { get; set; }
        [Required(ErrorMessage = "Выберите жанр")]
        public string? GenreName { get; set; }
        [Required(ErrorMessage = "Выберите автора")]
        public string? AuthorName { get; set; }

        [Required(ErrorMessage = "Введите кол-во страниц в книге")]
        public int? PagesCount { get; set; }
        [RegularExpression(price, ErrorMessage = "Введите корректную цену")]
        [DataType(DataType.Currency)]
        public string? Price { get; set; }
        [Required(ErrorMessage = "Введите описание")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(text, ErrorMessage = "Пожалуйста, используйте только русские буквы,цифры и знаки препинания")]
        [MaxLength(10000, ErrorMessage = "Длина описания не должна превышать 10000 символов"), MinLength(5, ErrorMessage = "Длина описания не должна быть меньше 10 символов")]
        public string? Description { get; set; }
        [DataType(DataType.Url)]
        [RegularExpression(url, ErrorMessage = "Введите корректный url")]
        public string? UrlEPUB { get; set; } = null!;
        [DataType(DataType.Url)]
        [RegularExpression(url, ErrorMessage = "Введите корректный url")]
        public string? UrlPDF { get; set; } = null!;
        [DataType(DataType.Url)]
        [RegularExpression(url, ErrorMessage = "Введите корректный url")]
        public string? ImageLink { get; set; } = null;

        public List<Author> Authors { get; set; } = new List<Author>();
        public List<Genre> Genres { get; set; } = new List<Genre>();
    }
}
