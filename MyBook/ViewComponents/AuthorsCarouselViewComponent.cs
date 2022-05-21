using Microsoft.AspNetCore.Mvc;
using MyBook.Entities;

namespace MyBook.ViewComponents;

public class AuthorsCarouselViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(List<Author> authors)
    {
        return View(authors);
    }
}