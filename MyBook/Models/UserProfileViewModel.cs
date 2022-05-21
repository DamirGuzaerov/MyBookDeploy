using MyBook.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBook.Models
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Введите email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указано имя"), DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Не указана фамилия"), DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Не указана дата рождения")]
        [DataType(DataType.Date, ErrorMessage = "Введите корректную дату рождения")]
        public DateTime? BirthDate { get; set; }

        public List<History> Histories { get; set; } = new List<History>();

        public List<Book> Recommendations { get; set; } = new List<Book>();
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }

    public class EditPasswordViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Введите старый пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Введите новый пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string NewPassword { get; set; }
    }
}
