using Microsoft.AspNetCore.Mvc;
using MyBook.Entities;

namespace MyBook.ViewComponents;

public class EditUserViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(User user)
    {
        return View(user);
    }
}