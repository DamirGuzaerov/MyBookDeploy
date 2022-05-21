using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBook.Core.Interfaces;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using MyBook.Models;
using System.Text.RegularExpressions;

namespace MyBook.Controllers
{
    /*[Authorize(Policy = "ReadersOnly")]*/
    public class UserProfileController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EFHistoryRepository _historyRepository;
        private readonly EFUserSubscrRepository _userSubscrRepository;
        private readonly IRecommendationsService _recommendationsService;
        public UserProfileController(UserManager<User> userManager, SignInManager<User> signInManager,
                                     EFUserSubscrRepository userSubscrRepository, EFHistoryRepository historyRepository,
                                     INotificationService notificationService, IRecommendationsService recommendationsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _historyRepository = historyRepository;
            _userSubscrRepository = userSubscrRepository;
            _notificationService = notificationService;
            _recommendationsService = recommendationsService;
        }

        public async Task<IActionResult> Index(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await DeleteSubscr(user.Id);
            return View(
                new UserProfileViewModel
                {
                    Id = id,
                    BirthDate = DateTime.Parse(user.BirthDate),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Histories = GetHistories(user.Id),
                    Recommendations = await GetRecommendations(user.Id,1),
                    Subscriptions = GetSubscrs(user.Id)
                });
        }

        private async Task DeleteSubscr(string userId)
        {
            var subscrs = _userSubscrRepository.GetExpiredUserSubscrs(userId);
            if (subscrs != null && subscrs.Any())
            {
                var message = "Упс у вас истекли следующие подписки: ";
                foreach (var subscrItem in subscrs)
                {
                    message += subscrItem.Subscription?.Type.TypeName;
                    if (subscrItem.Subscription?.Author != null)
                        message += $" {subscrItem.Subscription?.Author.Name}";
                    if (subscrItem.Subscription?.Genre != null)
                        message += $" {subscrItem.Subscription?.Genre.Name}";
                    message += ", ";
                }
                await _notificationService.NotifyClient(userId, "Обратите внимание", message);
            }
            await _userSubscrRepository.DeleteExpiredUserSubscrs(subscrs);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserProfileViewModel model)
        {
            model.Histories = GetHistories(model.Id);
            model.Subscriptions = GetSubscrs(model.Id);
            model.Recommendations = await GetRecommendations(model.Id,1);
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    if (_userManager.Users.Any(userIdentity => userIdentity.Email == model.Email && userIdentity.UserName != user.UserName))
                    {
                        ModelState.AddModelError(string.Empty, "Пользователь с таким email уже существует");
                        return View("Index", model);
                    }
                    if (model.BirthDate == null)
                    {
                        ModelState.AddModelError(string.Empty, "Введите корректную дату рождения");
                        return View("Index", model);
                    }
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.BirthDate = model.BirthDate?.ToShortDateString();
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return View("Index", model);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel model)
        {
            //TODO Почему то не отображается валидация
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", new { model.Id });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            if (error.Description == "Incorrect password.")
                                ModelState.AddModelError(string.Empty, "Введите верный старый пароль");
                            else ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return RedirectToAction("Index", new { model.Id });
        }

        public IActionResult AddVk(string userId)
        {
            var redirectUrl = Url.Action(nameof(AddVkCallback), "UserProfile", new { userId });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("VK", redirectUrl);
            return Challenge(properties, "VK");
        }

        public async Task<IActionResult> AddVkCallback(string userId)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("AddVK", new { userId });
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
            if (!result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var identityResult = await _userManager.AddLoginAsync(user, info);
            }
            //TODO else show that vk already added
            return RedirectToAction("Index", "UserProfile", new { id = userId });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private List<History> GetHistories(string userId)
        {
            return _historyRepository.GetHistories(userId).ToList();
        }

        private async Task<List<Book>> GetRecommendations(string userId,int page)
        {
            if (page < 1) page = 1;
            return await _recommendationsService.GetRecommendationsAsync(userId,page);
        }

        private async Task<PartialViewResult> GetRecommendationsPaginated(int page)
        {
            var userId = (await _userManager.GetUserAsync(User)).Id;
            var books =await GetRecommendations(userId, page);
            return PartialView("../Partials/_BooksList", books);
        }
        
        private List<Subscription> GetSubscrs(string userId)
        {
            return _userSubscrRepository.GetUserWithAllSubscr(userId).Select(it => it.Subscription).ToList();
        }
    }
}
