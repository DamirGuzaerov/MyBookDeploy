using Microsoft.AspNetCore.Mvc;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using MyBook.Models;
using System.Security.Claims;

namespace MyBook.Controllers
{
    public class AuthorController : Controller
    {
        private readonly EfAuthorRepository _authorRepository;
        private readonly EFUserRepository _userRepository;

        public AuthorController(EfAuthorRepository authorRepository, EFUserRepository userRepository)
        {
            _authorRepository = authorRepository;
            _userRepository = userRepository;
        }

        [Route("Author/{authorId:int}")]
        public async Task<IActionResult> Author(int authorId)
        {
            var resultAuthor = _authorRepository.GetFullAuthor(authorId);
            var model = new AuthorViewModel { Name = resultAuthor.Name, AuthorBooks = resultAuthor.AuthorBooks, BirthDate = resultAuthor.BirthDate, ImgLinks = resultAuthor.ImgLinks, HasSubscr = await CheckSubscr(resultAuthor.AuthorId) };
            return View(model);
        }

        private async Task<bool> CheckSubscr(int? authorId)
        {
            var user = _userRepository.GetUserWithSubscr(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
                return false;
            if (user.UserSubscrs != null && (user.UserSubscrs.Any(it =>
                it.Subscription.Type.TypeName == "Премиум" ||
                (authorId != null && it.Subscription.AuthorId == authorId))))
                return true;
            else return false;
        }
    }
}
