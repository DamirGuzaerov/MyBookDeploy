using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using MyBook.Models;
using MyBook.SharedKernel.Extensions;
using Repositories;


namespace MyBook.Controllers
{
    public class BookController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly EfBookRepository _bookRepository;
        private readonly EFUserRepository _userRepository;
        private readonly EFHistoryRepository _historyRepository;
        private readonly IGenericRepository<MyBook.Entities.Type> _typeRepository;
        private BookViewModel _viewModel;
        private readonly IGenericRepository<DownloadLink> _linksRepository;
        private readonly IGenericRepository<Rating> _ratingsRepository;

        public BookController(EfBookRepository bookRepository,
            EFUserRepository userRepository,
            EFTypeRepository typeRepository,
            IGenericRepository<DownloadLink> linksRepository,
            IGenericRepository<Rating> ratingsRepository,
            EFHistoryRepository historyRepository,
            UserManager<User> userManager,
            ILogger logger)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _typeRepository = typeRepository;
            _linksRepository = linksRepository;
            _ratingsRepository = ratingsRepository;
            _historyRepository = historyRepository;
            _userManager = userManager;
            _logger = logger;
        }

        // GET
        [Route("Book/{bookId:int}")]
        public IActionResult Book(int bookId)
        {
            _viewModel = new BookViewModel(_historyRepository, _linksRepository,
                _typeRepository, _bookRepository, bookId, CheckUser(),_logger);
            return View(_viewModel);
        }

        private User? CheckUser()
        {
            var user = _userRepository.GetUserWithSubscr(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
                return null;
            return user;
        }


        public async Task<IActionResult> DownloadFile(string link, string name, string format, int bookId)
        {
            var user = CheckUser();
            if (user == null) return RedirectToAction("Login", "Account");
            var net = new System.Net.WebClient();
            var data = net.DownloadData(link);
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            var fileName = name + "." + format;
            var book = _bookRepository.GetFullBook(bookId);
            /*if (user == null || book == null)
                return RedirectToAction("error"); //TODO show error*/
            var history = new History
            {
                BookId = bookId,
                DateTime = DateTime.Now.ToString(),
                UserId = user.Id,
            };
            book.DownloadsCount += 1;
            await _bookRepository.Update(book);
            if (!_historyRepository.CheckHistory(user.Id, bookId))
                await _historyRepository.Create(history);
            return File(content, contentType, fileName);
        }

        public async Task<PartialViewResult> PostComment(int rating, string comment, int bookId)
        {
            var returnComment = new Rating();
            var user = await _userManager.GetUserAsync(User);
            var bookRatings = _bookRepository.GetWithInclude(book => book.BookId == bookId, book => book.Ratings)
                .First().Ratings;
            var newComment = new Rating()
            {
                Points = rating,
                ReviewText = comment,
                Book = _bookRepository.FindById(bookId).Result,
                BookId = bookId,
                User = user,
                UserId = user.Id
            };
            if (CollectionHelper<Rating>.Contains(bookRatings, rating1 => rating1.UserId == user.Id))
            {
                var currentRating = bookRatings.First(rating1 => rating1.UserId == user.Id);
                currentRating.Points = rating;
                currentRating.ReviewText = comment;
                await _ratingsRepository.Update(currentRating, null);
            }
            else
            {
                await _ratingsRepository.Create(newComment);
            }

            return PartialView("../Partials/_Comment", newComment);
        }
    }
}