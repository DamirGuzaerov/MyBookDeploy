using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBook.Entities;
using Repositories;

namespace MyBook.Controllers;

public class TopBooksController : Controller
{
    private readonly IGenericRepository<Book> _bookRepository;

    public TopBooksController(IGenericRepository<Book> bookRepository)
    {
        _bookRepository = bookRepository;
    }
    [Route("[controller]/{pageCount:int?}")]
    public IActionResult Index(int? pageCount)
    {
        var freeBooks =  _bookRepository.GetWithMultiIncluding(
            book => book,
            book => book.Description.Price == " ",
            books => 
                books.Include(book=>book.Description)
                    .Include(book => book.AuthorBooks)
                    .ThenInclude(authorBook =>  authorBook.Author)
                    .Include(book => book.ImgLinks));
        
        return View(freeBooks);
    }
}