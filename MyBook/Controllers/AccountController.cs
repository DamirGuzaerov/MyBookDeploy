using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBook.Entities;
using MyBook.Models;
using System.Globalization;
using System.Security.Claims;

namespace MyBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly MyBookContext _bookContext;

        const string dateFormat = "yyyy-MM-dd";
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, MyBookContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _bookContext = db;
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userManager.Users.Any(userIdentity => userIdentity.Email == model.Email))
                {
                    ModelState.AddModelError(string.Empty, "Пользователь с таким email уже существует");
                    return View(model);
                }
                if (model.BirthDate == null)
                {
                    ModelState.AddModelError(string.Empty, "Введите корректную дату рождения");
                    return View(model);
                }
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate?.ToShortDateString()
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Reader"));
                    if (user.Email == "lizagarkina5@gmail.com")
                    {
                        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Admin"));
                    }
                    var info = await _signInManager.GetExternalLoginInfoAsync();
                    if (info != null)
                    {
                        var identityResult = await _userManager.AddLoginAsync(user, info);
                    }
                    _bookContext.SaveChanges();
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new SignInModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
        #region VK
        public IActionResult ExternalLogin(string returnUrl)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("VK", redirectUrl);
            return Challenge(properties, "VK");
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login", new { returnUrl });
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            DateTime? date = info.Principal.FindFirstValue(ClaimTypes.DateOfBirth) != null ? DateTime.Parse(info.Principal.FindFirstValue(ClaimTypes.DateOfBirth)) : null;
            return View("Registration", new RegistrationModel
            {
                FirstName = info.Principal.FindFirstValue(ClaimTypes.Name),
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                BirthDate = date
            });
        }

        #endregion
    }
}
