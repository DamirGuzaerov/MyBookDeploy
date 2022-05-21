using MyBook.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBook.Models.Admin
{
    public class EditCenterViewModel
    {
        const string text = @"^([А-Я]|[а-яё]|[A-z]|[A-z]|\s|\d|[.,!?:;\-,\/,@])*$";
        const string phone = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
        public int? BookCenterId { get; set; }
        [Required(ErrorMessage = "Введите название")]
        [MaxLength(50, ErrorMessage = "Длина названия не должна превышать 50 символов"), MinLength(5, ErrorMessage = "Длина названия не должна быть меньше 5 символов")]
        [RegularExpression(text, ErrorMessage = "Введите корректное название", MatchTimeoutInMilliseconds = 10)]
        [DataType(DataType.Text)]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Введите адрес")]
        [MaxLength(50, ErrorMessage = "Длина адреса не должна превышать 50 символов"), MinLength(5, ErrorMessage = "Длина адреса не должна быть меньше 5 символов")]
        [RegularExpression(text, ErrorMessage = "Введите корректный адрес", MatchTimeoutInMilliseconds = 10)]
        [DataType(DataType.Text)]
        public string Address { get; set; } = null!;
        [Required(ErrorMessage = "Введите номер")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(phone, ErrorMessage = "Введите корректный номер")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "Введите описание")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(text, ErrorMessage = "Пожалуйста, используйте буквы,цифры и знаки препинания")]
        [MaxLength(500, ErrorMessage = "Длина описания не должна превышать 500 символов"), MinLength(5, ErrorMessage = "Длина описания не должна быть меньше 10 символов")]
        public string Description { get; set; } = null!;
    }
}
