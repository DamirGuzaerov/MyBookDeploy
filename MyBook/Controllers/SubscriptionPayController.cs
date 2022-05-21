using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBook.Core.Interfaces;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using MyBook.Models;
using Repositories;
using System.Security.Claims;

namespace MyBook.Controllers
{
    public class SubscriptionPayController : Controller
    {
        private readonly IGenericRepository<Object> _genericRepository;
        private readonly IPaymentService _paymentService;
        private readonly EFHistoryRepository _historyRepository;

        public SubscriptionPayController(IGenericRepository<Object> genericRepository, IPaymentService paymentService, EFHistoryRepository historyRepository)
        {
            _genericRepository = genericRepository;
            _paymentService = paymentService;
            _historyRepository = historyRepository;
        }
        
        public IActionResult SubscriptionPay(PayViewModel model)
        {
            return View(model);
        }

        [Authorize]
        public IActionResult BookPay(int bookId, string price, string name)
        {
            return View(
                new PayViewModel { BookId = bookId, UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), BookPrice = price, BookName = name});
        }

        [HttpPost]
        public IActionResult Pay(PayViewModel model)
        {
            return View("PaymentModal", model);
        }

        //Пример корректного номера карты 4111111111111111
        [HttpPost]
        public async Task<IActionResult> PaymentModal(PayViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.CardNum) || string.IsNullOrWhiteSpace(model.CardCode) || string.IsNullOrWhiteSpace(model.CardDate) || string.IsNullOrWhiteSpace(model.CardName))
                {
                    ModelState.AddModelError("EmptyFields", "Заполните все поля");
                    return View(model);
                }
                if (!_paymentService.PerformPayment())
                {
                    ModelState.AddModelError("Unsuccessfull payment", "Оплата была отклонена");
                    return View(model);
                }
                if(model.UserId == null)
                    return Redirect("Error"); //TODO show error
                if (model.BookId != null)
                {
                    var history = new History
                    {
                        BookId = (int)model.BookId,
                        DateTime = DateTime.Now.ToString(),
                        UserId = model.UserId,
                    };
                    if(!_historyRepository.CheckHistory(model.UserId,(int)model.BookId))
                        await _historyRepository.Create(history);
                    return RedirectToAction("Book", "Book", new { model.BookId });
                }
                if (model.Period == null || model.TypeId == null)
                    return Redirect("Error"); //TODO show error
                var subscr = new Subscription
                {
                    StartDate = DateTime.Now.ToString(),
                    EndDate = DateTime.Now.AddMonths((int)model.Period).ToString(),
                    TypeId = (int)model.TypeId,
                };
                if (model.TypeName == "Подписка на автора")
                    subscr.AuthorId = model.SpecsId;
                if (model.TypeName == "Подписка на жанр")
                    subscr.GenreId = model.SpecsId;
                var userSubscr = new UserSubscr { Subscription = subscr, UserId = model.UserId };
                subscr.UserSubscr = userSubscr;
                await _genericRepository.CreateAll(new List<object>() { subscr, userSubscr, });
                //TODO show modal that subscr succesfully added
                if (model.isGift)
                    return RedirectToAction("PaySuccess", "SubscriptionGift", new { userId = model.UserId });
                return RedirectToAction("Subscription", "Subscription");
            }
            return View(model);
        }
    }
}