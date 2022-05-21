using Microsoft.AspNetCore.Mvc;
using MyBook.Entities;

namespace MyBook.ViewComponents;

public class AuthorSmallPreviewViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(Author author)
    {
        return View(author);
    }
}