using Microsoft.AspNetCore.Mvc;
using MyBook.Entities;

namespace MyBook.ViewComponents;

public class EditAuthorViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(Author author)
    {
        return View(author);
    }
}