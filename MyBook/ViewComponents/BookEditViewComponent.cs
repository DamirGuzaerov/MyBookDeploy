using Microsoft.AspNetCore.Mvc;
using MyBook.Entities;

namespace MyBook.ViewComponents;

public class BookEditViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(Book book)
    {
        return View(book);
    }
}