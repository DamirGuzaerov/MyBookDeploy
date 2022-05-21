using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using MyBook.Models;
using Repositories;
using System.Security.Claims;

namespace MyBook.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IGenericRepository<Genre> _genreRepository;
        private readonly IGenericRepository<MyBook.Entities.Type> _typeRepository;
        private readonly EFUserRepository _userRepository;


        public SubscriptionController(IGenericRepository<Author> authorRepository, IGenericRepository<Genre> genreRepository,
            IGenericRepository<MyBook.Entities.Type> typeRepository, EFUserRepository userRepository)
        {
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _typeRepository = typeRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Subscription()
        {
            return View(new BuySubscrViewModel { Genres = GetGenres(), Authors = GetAuthors(), SubscrTypes = GetTypes() });
        }

        [HttpGet]
        public IActionResult SubscrForGenre()
        {
            return RedirectToAction("Subscription");
        }

        [Authorize(Policy = "ReadersOnly")]
        [HttpPost]
        public async Task<IActionResult> SubscrForGenre(string GenreName)
        {
            var type = GetTypes().FirstOrDefault(it => it.TypeName == "Подписка на жанр");
            var genre = _genreRepository.Get(it => it.Name == GenreName);
            var model = new PayViewModel();
            if (type != null)
            {
                var user = await CheckSubscr(type.TypeId, genreId: genre.First().GenreId, authorId: null);
                if (user == null || genre == null)
                    return Redirect("Error");//TODO show modal that user already have subscr
                model = new PayViewModel
                {
                    UserId = user.Id,
                    Period = 1,
                    SpecsName = genre.First().Name,
                    SpecsId = genre.First().GenreId,
                    TypeId = type.TypeId,
                    TypeName = type.TypeName,
                    Price = type.Price,
                };
                return RedirectToAction("SubscriptionPay", "SubscriptionPay", model);
            }
            return RedirectToAction("SubscriptionPay", "SubscriptionPay", model);
        }

        [Authorize(Policy = "ReadersOnly")]
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> SubscrForAuthor(string AuthorName)
        {
            if (AuthorName == null)
                return RedirectToAction("Subscription");
            var type = GetTypes().FirstOrDefault(it => it.TypeName == "Подписка на автора");
            var author = _authorRepository.Get(it => it.Name == AuthorName);
            var model = new PayViewModel();
            if (type != null)
            {
                var user = await CheckSubscr(type.TypeId, authorId: author.First().AuthorId, genreId: null);
                if (user == null || author == null)
                    return Redirect("Error");//TODO show modal that user already have subsc
                model = new PayViewModel
                {
                    UserId = user.Id,
                    Period = 1,
                    SpecsName = author.First().Name,
                    SpecsId = author.First().AuthorId,
                    TypeId = type.TypeId,
                    TypeName = type.TypeName,
                    Price = type.Price,
                };
                return RedirectToAction("SubscriptionPay", "SubscriptionPay", model);
            }
            return RedirectToAction("SubscriptionPay", "SubscriptionPay", model);
        }

        [Authorize(Policy = "ReadersOnly")]
        public async Task<IActionResult> SubscrForPremium()
        {
            var type = GetTypes().FirstOrDefault(it => it.TypeName == "Премиум");
            var model = new PayViewModel();
            if (type != null)
            {
                var user = await CheckSubscr(type.TypeId, null, null);
                if (user == null)
                    return Redirect("Error");//TODO show modal that user already have subscr
                model = new PayViewModel
                {
                    UserId = user.Id,
                    Period = 1,
                    SpecsName = null,
                    SpecsId = null,
                    TypeId = type.TypeId,
                    TypeName = type.TypeName,
                    Price = type.Price,
                };
                return RedirectToAction("SubscriptionPay", "SubscriptionPay", model);
            }
            return RedirectToAction("SubscriptionPay", "SubscriptionPay", model);
        }

        private async Task<User?> CheckSubscr(int typeId, int? genreId, int? authorId)
        {
            var user = _userRepository.GetUserWithSubscr(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
                return null;
            if (user.UserSubscrs == null || !(user.UserSubscrs.Any(it =>
                it.Subscription.TypeId == typeId &&
                ((genreId != null && it.Subscription.GenreId == genreId) || (authorId != null && it.Subscription.AuthorId == authorId) || it.Subscription.Type.TypeName == "Премиум"))))
                return user;
            else return null;
        }

        private List<Genre> GetGenres()
        {
            return _genreRepository.Get().ToList();
        }

        private List<MyBook.Entities.Type> GetTypes()
        {
            return _typeRepository.Get().ToList();
        }

        private List<Author> GetAuthors()
        {
            return _authorRepository.Get().ToList();
        }
    }
}