using System.ComponentModel.DataAnnotations;

namespace MyBook.Models.Admin
{
    public class EditSubscrViewModel
    {
        const string letters = @"^([А-Я]([а-яё]|\s)*)";
        const string text = @"^([А-Я]|[а-яё]|[A-z]|[A-z]|\s|\d|[.,!?:;-])*";
        const string price = @"^((\d{1,3}|\s*){1})((\,\d{3}|\d)*)(\s*|\.(\d{2}))$";
        public int? TypeId { get; set; }
        [Required(ErrorMessage = "Введите название типа подписки")]
        [MaxLength(50, ErrorMessage = "Длина названия не должна превышать 50 символов"), MinLength(5, ErrorMessage = "Длина названия не должна быть меньше 5 символов")]
        [RegularExpression(letters, ErrorMessage = "Название должно состоять только из букв и начинаться с заглавной")]
        [DataType(DataType.Text)]
        public string TypeName { get; set; }
        [Required(ErrorMessage = "Введите цену")]
        [RegularExpression(price, ErrorMessage = "Введите корректную цену")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }
        [Required(ErrorMessage = "Введите описание")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(text, ErrorMessage = "Пожалуйста, используйте только русские буквы,цифры и знаки препинания")]
        [MaxLength(200, ErrorMessage = "Длина описания не должна превышать 200 символов"), MinLength(5, ErrorMessage = "Длина описания не должна быть меньше 10 символов")]
        public string? Description { get; set; }
    }
}
