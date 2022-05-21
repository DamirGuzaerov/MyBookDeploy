using MyBook.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBook.Models.Admin
{
    public class EditAuthorViewModel
    {
        const string letters = @"^([A-Za-z]|[А-Яа-яё])+((\s)?((\'|\-|\.)?([A-Za-z]|[А-Яа-яё])+))*$";
        const string url = @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$";
        public int? AuthorId { get; set; }
        [Required(ErrorMessage = "Введите имя автора")]
        [MaxLength(50, ErrorMessage = "Длина имени не должна превышать 50 символов"), MinLength(5, ErrorMessage = "Длина имени не должна быть меньше 5 символов")]
        [RegularExpression(letters, ErrorMessage = "Введите корректное имя", MatchTimeoutInMilliseconds = 10)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Введите корректную дату рождения")]
        public DateTime? BirthDate { get; set; } = null;

        [DataType(DataType.Url)]
        [RegularExpression(url, ErrorMessage = "Введите корректный url")]
        public string? ImageLink { get; set; } = null;
    }
}
