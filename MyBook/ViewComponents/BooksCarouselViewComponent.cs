using Microsoft.AspNetCore.Mvc;
using MyBook.Entities;

namespace MyBook.ViewComponents;

public class BooksCarouselViewComponent: ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(List<Book> book)
    {
        return Task.FromResult<IViewComponentResult>(View(book));
    }
}