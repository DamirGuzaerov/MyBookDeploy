using Microsoft.AspNetCore.Mvc;
using MyBook.Models;
using System.Diagnostics;
using MyBook.Entities;
using Repositories;
using Microsoft.AspNetCore.SignalR;
using MyBook.Infrastructure.Hubs;
using MyBook.Core.Interfaces;
using System.Security.Claims;
using MyBook.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace MyBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenericRepository<BookCenter> _bookCenterRepository;
        private readonly INotificationService _notificationService;
        private readonly EFUserSubscrRepository _userSubscrRepository;
        public HomeController(IGenericRepository<BookCenter> bookCenterRepository, INotificationService notificationService, EFUserSubscrRepository userSubscrRepository)
        {
            _bookCenterRepository = bookCenterRepository;
            _notificationService = notificationService;
            _userSubscrRepository = userSubscrRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> CheckUserSubscr()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Json(true);
            if (HttpContext.Session.Keys.Contains("isNotificated") && HttpContext.Session.GetString("isNotificated") == "true")
                return Json(true);
            var subscr = _userSubscrRepository.GetExpiredUserSubscrs(userId, 5);
            if (subscr != null && subscr.Any())
            {
                var message = "В течение пяти дней у Вас истекают следующие подписки: ";
                foreach (var subscrItem in subscr)
                {
                    message += subscrItem.Subscription?.Type.TypeName;
                    if (subscrItem.Subscription?.Author != null)
                        message += $" {subscrItem.Subscription?.Author.Name}";
                    if (subscrItem.Subscription?.Genre != null)
                        message += $" {subscrItem.Subscription?.Genre.Name}";
                    message += ", ";
                }
                await _notificationService.NotifyClient(userId, "Обратите внимание", message);
                HttpContext.Session.SetString("isNotificated", "true");
            }
            return Json(true);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public JsonResult GetBooksCenter()
        {
            var bookCenters = _bookCenterRepository.Get();
            return Json(bookCenters);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}