using Microsoft.AspNetCore.Identity;
using MyBook.Entities;
using System.Text.RegularExpressions;

namespace MyBook.Validation
{
    public class UserValidator : IUserValidator<User>
    {
        DateTime minDate = DateTime.Parse("06.04.1922");
        const string letters = @"^([А-Я][а-яё]{2,50}|[A-Z][a-z]{2,50})$";
        const string email = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if (!Regex.IsMatch(user.Email, email))
            {
                errors.Add(new IdentityError
                {
                    Description = "Email должен быть действителен"
                });
            }
            if (!Regex.IsMatch(user.FirstName,letters)|| !Regex.IsMatch(user.LastName, letters))
            {
                errors.Add(new IdentityError
                {
                    Description = "Имя и фамилия должны состоять только из букв и начинаться с большой буквы"
                });
            }
            var date = DateTime.Parse(user.BirthDate);
            if (date.CompareTo(DateTime.Now.Date)>0 || date.CompareTo(minDate) < 0) 
            {
                errors.Add(new IdentityError
                {
                    Description = "Введите корректную дату рождения"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
